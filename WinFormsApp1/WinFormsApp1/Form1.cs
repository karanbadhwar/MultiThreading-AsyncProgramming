namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }       

        private async Task button1_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(() => ShowMessage("First message", 3000));
            //thread.Start();
            await ShowMessage("First Message", 3000);
        }

        private async Task button2_Click(object sender, EventArgs e)
        {
            await ShowMessage("Second message", 5000);
        }

        private async Task ShowMessage(string message, int delay)
        {
            await Task.Delay(delay);
            LblMessage.Text = message;
        }
    }
}
