using System;
using KittyHelper.ServiceGenerators.CS;
using ServiceStack.Script;
using static KittyHelper.KittyHelper.KittyViewHelper;
using static KittyHelper.ServiceGenerators.KittyServiceHelper;

namespace KittyHelper
{
    public class CreateUpdateEndPoint<T> : ICreateAnEndPoint
    {
        private CreateUpdateEndPointOptions<T> options;
        private readonly GenerateEndPointAuthHelper<T> helper;

        public CreateUpdateEndPoint(CreateUpdateEndPointOptions<T> options)
        {
            this.options = options;
            helper = new GenerateEndPointAuthHelper<T>(options);
        }


        public virtual CStyleClass Create()
        {
            var createFunctionParameters = new CStyleParameter[]
            {
                new CStyleParameter("request",
                    new CStyleTypeDeclaration(options.RequestObjectType))
            };
            var createFunction = GenerateUpdateFunction(createFunctionParameters);
            var usings = GenerateUsings();
            var parentClass = new CStyleClass("ServiceStack.Service", options.ServiceObjectNamespace, Array.Empty<string>());
            var createClass =
                new CStyleClass(GenerateFunctionName(), options.ServiceObjectNamespace, usings: usings,
                    extends: parentClass,
                    functions: new[]
                    {
                        createFunction
                    });


            return createClass;
        }

        protected virtual string GenerateFunctionName()
        {
            return "Create" + typeof(T).Name;
        }

        protected virtual CStyleFunction GenerateUpdateFunction(CStyleParameter[] createFunctionParameters)
        {
            var createFunction =
                new CStyleFunction(options.HttpVerb,
                    new CStyleTypeDeclaration(options.ResponseObjectType), true,
                    cStyleParameters: createFunctionParameters,
                    new[]
                    {
                        new CStyleTryCatchFinally(new CStyleStatement[]
                        {
                            new CStyleStatement(helper.GenerateUserLookUp()),
                            new CStyleStatement(helper.GenerateAssignToUser()),
                            GenerateDatabaseMethod(),
                            "return ", GenerateReturnObject(),
                            ";"
                        }, new CStyleStatement[]
                        {
                            "return ", GenerateReturnObjectOnError(),
                            ";"
                        })
                    }
                );
            return createFunction;
        }

        protected virtual CStyleObject GenerateReturnObjectOnError()
        {
            return new CStyleObject(options.ResponseObjectType,
                new CStyleObjectInitalizer[]
                {
                    new CStyleObjectInitalizer("Success", "false"),
                    new CStyleObjectInitalizer("Message", "e.Message"),
                });
        }

        private CStyleObject GenerateReturnObject()
        {
            return new CStyleObject(options.ResponseObjectType,
                new CStyleObjectInitalizer[]
                {
                    new CStyleObjectInitalizer("Count", "count")
                });
        }

        protected virtual CStyleStatement GenerateDatabaseMethod()
        {
            return
                $" var count= Db.Update( {options.RequestObjectName}.{options.RequestObjectUpdateObjectField});";
        }
        protected virtual string[] GenerateUsings()
        {
            string[] usings = new string[]
            {
                options.RequestObjectNamespace,
                "Microsoft.AspNetCore.Hosting",
                "ServiceStack",
                "ServiceStack.Auth",
                "ServiceStack.Configuration",
                "ServiceStack.Logging",
                "System.Linq",
                "ServiceStack.OrmLite",
                "ServiceStack.FluentValidation",
                "System",
                "System.Threading.Tasks",
            };
            return usings;
        }

        public virtual CStyleClass CreateRequestClass()
        {
            var usings = new string[]
            {
                "System",
                "ServiceStack" ,
                typeof(T).Namespace
            };
            var extends = new
                CStyleClass(
                    $"IReturn<{options.ResponseObjectType}>", options.RequestObjectNamespace);

            var requestObjectFields = new[]
            {
                new CStyleClassField(options.RequestObjectUpdateObjectField,
                    new CStyleTypeDeclaration(typeof(T).Name)),
            };

            return new CStyleClass(options.RequestObjectType,
                options.RequestObjectNamespace,
                
                usings: usings,
                extends: extends,
                fields: requestObjectFields);
        }

        public virtual CStyleClass CreateResponseType()
        {
            var usings = new string[]
            {
                "System"
            };
            var responseObjectFields = new[]
            {
                new CStyleClassField("Count",
                    new CStyleTypeDeclaration("long")),
                new CStyleClassField("Message",
                    new CStyleTypeDeclaration("string")),
                new CStyleClassField("Success",
                    new CStyleTypeDeclaration("bool")),
            };
            return new(options.ResponseObjectType, options.ResponseObjectNamespace, usings: usings, fields:
                responseObjectFields);
        }
    }
}