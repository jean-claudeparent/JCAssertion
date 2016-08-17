using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCASQLODPCore
{
    public class JCASQLODPException : Exception 
    {
         public JCASQLODPException()
        {

        }

        public JCASQLODPException(string message)
            : base(message)
        {
        }

        public JCASQLODPException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
