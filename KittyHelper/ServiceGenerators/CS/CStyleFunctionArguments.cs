using System;
using KittyHelper.ServiceGenerators.CS;

namespace KittyHelper
{
   
            public class CStyleFunctionArguments : CStyleStatement
            {
             
                private CStyleStatement requestArgument;

                public CStyleFunctionArguments(CStyleStatement requestArgument)
                {
                    this.requestArgument = requestArgument;
                }

                public CStyleFunctionArguments(CStyleObject requestArgument)
                {
                    this.requestArgument = requestArgument;
                }

                public override string Render()
                {
                    return requestArgument.Render();
                }
            }
        }
 