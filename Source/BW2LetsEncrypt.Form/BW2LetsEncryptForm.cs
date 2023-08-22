using LetsEncrypt.Client;
using LetsEncrypt.Client.Entities;
using LetsEncrypt.Client.Interfaces;
using LetsEncrypt.Client.IO;
using LetsEncrypt.Client.Loggers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BW2LetsEncrypt.FormApp
{
    public partial class BW2LetsEncryptForm : Form
    {
        #region Fields + Properties

        private static readonly Lazy<IServiceProvider> _serviceProvider = new Lazy<IServiceProvider>(InitDependencyInjection);

        private static IServiceProvider ServiceProvider => _serviceProvider.Value;

        private static ILogger Logger => ServiceProvider.GetRequiredService<ILogger>();

        public static LocalStorage LocalFileHandler => ServiceProvider.GetRequiredService<LocalStorage>();

        public static Settings Settings => ServiceProvider.GetRequiredService<Settings>();

        #endregion Fields + Properties

        #region Constructor

        public BW2LetsEncryptForm()
        {
            InitValues().GetAwaiter();
            InitializeComponent();
            progressBarCert.Value = 0;
        }
        AcmeClient acmeClient;
        Account account;
        Order order;
        List<Challenge> challenges;
        public async Task InitValues()
        {
            InitDependencyInjection();
            // Instantiate and set up ACME client 
            acmeClient = new AcmeClient(ApiEnvironment.LetsEncryptV2Staging);
            account = await acmeClient.CreateNewAccountAsync(Settings.ContactEmail);
            order = await acmeClient.NewOrderAsync(account, Settings.Domains);
            //challenges = await acmeClient.GetDnsChallenges(account, order);
            challenges = await acmeClient.GetDnsChallenges(account, order);
            AppendToLog("Step 0 - ACME client already setup , please start the process to generate the certificate");
            progressBarCert.Value = 10;
            this.btn_CreateCertificate.Enabled = true;
            this.LblProcessStep.Text = string.Empty;
            txt_answer.ScrollBars = ScrollBars.Vertical;
        }

        #endregion Constructor



        #region Dependency Injection Initialization

        private static IServiceProvider InitDependencyInjection()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<LocalStorage>();
            services.AddSingleton<Settings>();

            return services.BuildServiceProvider();
        }

        #endregion Dependency Injection Initialization

        #region Event Handlers

        private async void btn_CreateCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                await RunProcessPart1(acmeClient, challenges, account, order);
            }
            catch (Exception ex)
            {
                AppendToLog(ex.Message, true);
                ResetOperation();
            }

        }

        private async void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                await RunProcessPart2(acmeClient, challenges, account, order);
            }
            catch (Exception ex)
            {
                AppendToLog(ex.Message, true);
                ResetOperation();
            }

        }

        private void ResetOperation()
        {
            AppendToLog("=========================================" + Environment.NewLine);
            this.btn_CreateCertificate.Enabled = true;
            this.btnContinue.Enabled = false;
            this.LblProcessStep.Text = string.Empty;
            this.progressBarCert.Value = 10;
        }
        #endregion Event Handlers

        #region Utility Methods

        public void AppendToLog(string message, bool bold = false)
        {
            if (bold)
            {
                txt_answer.Font = new Font(txt_answer.Font, FontStyle.Bold);
            }
            else
            {
                txt_answer.Font = new Font(txt_answer.Font, FontStyle.Regular);
            }

            txt_answer.AppendText(message + Environment.NewLine);
        }

        private async Task RunProcessPart1(AcmeClient acmeClient, List<Challenge> challenges, Account account, Order order)
        {
            this.LblProcessStep.Text = "Processing...";
            AppendToLog("Step 1 - Order Creation");
            AppendToLog("Step 1 - Done");
            AppendToLog("Step 2 - Verification by DNS challenge");

            var sb = new StringBuilder(256);
            foreach (var challenge in challenges)
            {
                AppendToLog($"DNS TXT record Key: {challenge.DnsKey}");
                AppendToLog($"DNS TXT record Value: {challenge.VerificationValue}");
            }
            await LocalFileHandler.WriteAsync("_Output.txt", sb.ToString());

            AppendToLog("Step 2 - Please configure the DNS TXT record(s)");
            AppendToLog("Step 2 - Press the button to continue ...");
            btn_CreateCertificate.Enabled = false;
            btnContinue.Enabled = true;
            progressBarCert.Value = 50;
        }

        private async Task RunProcessPart2(AcmeClient acmeClient, List<Challenge> challenges, Account account, Order order)
        {
            AppendToLog("Step 3 - Verification of DNS TXT record(s)");

            // Validation of all DNS entries
            var failedCount = 3;
            var valid = false;
            while (!valid)
            {
                try
                {
                    foreach (var challenge in challenges)
                    {
                        await acmeClient.ValidateChallengeAsync(account, challenge);

                        // Verify status of challenge
                        var freshChallenge = await acmeClient.GetChallengeAsync(account, challenge);
                        if (freshChallenge.Status == ChallengeStatus.Invalid)
                        {
                            AppendToLog("Something is wrong with your DNS TXT record(s)!", true);
                            //throw new Exception("Something is wrong with your DNS TXT record(s)!");
                        }
                    }

                    valid = true;
                }
                catch (Exception ex)
                {
                    failedCount--;

                    if (failedCount == 0)
                    {
                        AppendToLog("Validation of DNS TXT record(s) is failed!");
                        throw new Exception("Validation of DNS TXT record(s) is failed!", ex);
                    }

                    Thread.Sleep(5000);
                }
            }

            AppendToLog("Step 3 - Done");
            AppendToLog("Step 4 - Certificate generation");
            Thread.Sleep(3000);
            // Generate certificate
            var certificate = await acmeClient.GenerateCertificateAsync(account, order, Settings.CertificateFileName);
            // Save files locally
            await LocalFileHandler.WriteAsync(Settings.CertificateFileName + ".pfx", certificate.GeneratePfx(Settings.CertificatePassword));
            await LocalFileHandler.WriteAsync(Settings.CertificateFileName + ".crt", certificate.GenerateCrt(Settings.CertificatePassword));
            await LocalFileHandler.WriteAsync(Settings.CertificateFileName + ".crt.pem", certificate.GenerateCrtPem(Settings.CertificatePassword));
            await LocalFileHandler.WriteAsync(Settings.CertificateFileName + ".key.pem", certificate.GenerateKeyPem());
            AppendToLog("Step 4 - Done");
            btnContinue.Enabled = false;
            btn_CreateCertificate.Enabled = true;
            progressBarCert.Value = 100;
            MessageBox.Show("End proccess");
            this.LblProcessStep.Text = "End process";
        }
        #endregion Utility Methods

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}