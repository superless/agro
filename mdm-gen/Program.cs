using Colorful;
using CommandLine;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;

namespace mdm_gen
{
    public class Program
    {
        [Flags]
        public enum GenKind{ 
            data = 0,            
            model = 1,
        }

        public enum BranchType { 
            production,
            release,
            hotfix,
            develop
        }
        

        [Verb("typescript", HelpText = "Generación de mdm para typescript")]
        public class TypeScriptArguments {


            [Option('m', "model-namespace", Required = false, HelpText = "namespace del modelo de clases")]
            public string modelNamespace { get; set; }

            [Option('i', "input-namespace", Required = false, HelpText = "namespace de las clases input")]
            public string inputNamespace { get; set; }


            [Option('d', "docs-namespace", Required = false, HelpText = "namespace donde se encuentre la implementación de IMdmDocumentation")]
            public string docsNamespace { get; set; }


            [Option('a', "assembly", Required = false, HelpText = "ruta del assembly")]
            public string Assembly { get; set; }

            [Option('t', "type", Required = false, HelpText = "tipo de generación, si es del modelo y no se indica el namespace, ni el ")]
            public GenKind GenKind { get; set; } = GenKind.model;

            [Value(0, Required = true, HelpText = "Url o ssh de la url de git del proyecto, esto permitirá modificar la rama y gatillar la generación de una nueva versión del paquete (si esta configurado el pipeline)")]
            public string GitAddress { get; set; }

            [Option('u', "user", Required = true, HelpText = "Usuario que registra el cambio en el repositorio del componente")]
            public string username { get; set; }

            [Option('e', "email", Required = true, HelpText = "correo que registra el cambio en el repositorio del componente")]
            public string email { get; set; }



        }

       
        // entry point into console app
        static void Main(string[] args)
        {


            var result = Parser.Default.ParseArguments<TypeScriptArguments, object>(args);
            result.WithParsed<TypeScriptArguments>(ProcessArgs);
            result.WithParsed<object>((obj)=> { });


        }

        public static void ProcessArgs(TypeScriptArguments ts) {

            var fontTitle = new Figlet(Colorful.FigletFont.Load("figlet/small"));            
            
            var mdmFiglet = new Figlet(Colorful.FigletFont.Load("figlet/small"));


            Colorful.Console.WriteLine(fontTitle.ToAscii("Trifenix Connect"), Color.Red);
            
            Colorful.Console.WriteLine(mdmFiglet.ToAscii("MDM"), Color.Purple);

            Colorful.Console.WriteLine("Bienvenido a la generación de código de trifenix connect mdm", Color.BlueViolet);

            Colorful.Console.WriteLine("Usted ha seleccionado la generación de paquetes de Typescript", Color.DarkGreen);

            if (ts.GenKind == GenKind.model && string.IsNullOrWhiteSpace(ts.modelNamespace))
            {
                Colorful.Console.WriteLine("Generación de paquete con los tipos base de MDM", Color.DarkGreen);
                CreateBaseModelPackage(ts.GitAddress, ts.email, ts.username);
            }
            else if (ts.GenKind == GenKind.data) {

                Colorful.Console.WriteLine("Generación datos del modelo", Color.DarkGreen);
                CreateDataModel(ts.Assembly, ts.modelNamespace, ts.inputNamespace, ts.docsNamespace, ts.GitAddress, ts.username, ts.email);

                
            }



            //else if (ts.GenKind == GenKind.model && ts.Namespaces != null && ts.Namespaces.Any() && string.IsNullOrWhiteSpace(ts.Assembly))
            //{
            //    Colorful.Console.WriteLine("Generación de paquete con los tipos base de un assembly y sus namespaces", Color.Bisque);

            //}
        }

        public static void CreateModelAssembly() { 
            
        }

        public static void CreateBaseModelPackage(string gitAddress, string email, string username) {
            MdmGen.GenerateMdm(gitAddress, email, username);
        }

        public static void CreateDataModel(string assembly, string modelNamespace, string inputNamespace, string documentNamespace, string gitRepo, string user, string email) {
            MdmGen.GenerateDataMdm(assembly, modelNamespace, inputNamespace, documentNamespace, gitRepo, user, email);
        }


    }

    
}
