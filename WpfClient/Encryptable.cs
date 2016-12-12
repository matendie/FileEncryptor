using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient
{
    public class Encryptable
    {
        public event EventHandler ValueChanged;
        public delegate void AlarmEventHandler(object sender, EventArgs e);

        public int Index { get; set; }
        private string _name { get; set; }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnValueChanged(new EventArgs());
            }
        }
        public string FilePath { get; set; }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }
    }
}
