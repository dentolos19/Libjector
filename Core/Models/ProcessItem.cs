namespace WxInjector.Core.Models
{

    internal class ProcessItem
    {

        public readonly int Process = -1;
        public readonly string Text;

        public ProcessItem(string text, int process)
        {
            Text = text;
            Process = process;
        }

        public override string ToString()
        {
            return Text;
        }

    }

}