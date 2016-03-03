using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JCAExporte
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                String Message = "";
                int CodeDeRetour = 0;
                JCAExporteCore monJCAExporteCore = new JCAExporteCore();

                CodeDeRetour = monJCAExporteCore.ExecuteExporte(args , out Message );


                Console.WriteLine(Message );
                Environment.Exit(CodeDeRetour );
             } catch  (Exception excep)
                {
                    Console.WriteLine(excep.Message);
                    Environment.Exit(99);
            
                }

        }
    }
}
