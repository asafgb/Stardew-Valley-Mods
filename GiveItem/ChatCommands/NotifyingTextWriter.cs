using System;
using System.IO;
using System.Text;


namespace ChatCommands
{
    public class NotifyingTextWriter : TextWriter
    {
        private readonly NotifyingTextWriter.OnLineWritten callback;
        private readonly TextWriter original;
        private bool isForceNotifying;
        private bool isNotifying;
        public delegate void OnLineWritten(char[] buffer, int index, int count);

        public NotifyingTextWriter(TextWriter original, NotifyingTextWriter.OnLineWritten callback)
        {
            this.original = original;
            this.callback = callback;
        }

        public override Encoding Encoding
        {
            get
            {
                return this.original.Encoding;
            }
        }
        
        public override void Write(char[] buffer, int index, int count)
        {
            bool flag = this.isNotifying || this.isForceNotifying;
            if (flag)
            {
                this.callback(buffer, index, count);
            }
            this.original.Write(buffer, index, count);
        }
        
        public override void Write(char ch)
        {
            this.original.Write(ch);
        }

        
        public bool IsForceNotifying()
        {
            return this.isForceNotifying;
        }

        public bool IsNotifying()
        {
            return this.isNotifying;
        }

        public void Notify(bool b)
        {
            this.isNotifying = b;
        }
        
        public void ToggleForceNotify()
        {
            this.isForceNotifying = !this.isForceNotifying;
        }
    }
}
