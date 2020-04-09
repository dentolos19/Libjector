namespace WxInjector.Core.Models
{

    internal class ProcessItem
    {

        public int Process = -1;
        public string Text;

        public ProcessItem(string Text, int Process)
        {
            this.Text = Text;
            this.Process = Process;
        }

        public override string ToString()
        {
            return Text;
        }

    }

}