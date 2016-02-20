using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCAssertionCore
{
    public class JCAssertionException : Exception
    {
        public JCAssertionException()
        {

        }

        public JCAssertionException(string message)
            : base(message)
        {
        }

        public JCAssertionException(string message, Exception inner)
            : base(message, inner)
        {

        }

       }
}
