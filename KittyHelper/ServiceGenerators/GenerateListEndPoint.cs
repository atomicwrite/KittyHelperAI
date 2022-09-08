using System;
using System.Collections.Generic;
using System.Text;
using KittyHelper.Options;
using KittyHelper.ServiceGenerators.CS;
using ServiceStack.Script;
using static KittyHelper.KittyHelper.KittyViewHelper;
using static KittyHelper.ServiceGenerators.KittyServiceHelper;

namespace KittyHelper
{
    public class CreateListEndPoint<T> : ICreateAnEndPoint
    {
        private CreateListEndPointOptions<T> options;
        private readonly GenerateEndPointAuthHelper<T> helper;

        public CreateListEndPoint(CreateListEndPointOptions<T> options)
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
            var createFunction = GenerateListFunction(createFunctionParameters);
            var usings = GenerateUsings();
            var parentClass = new CStyleClass("Service", options.ServiceObjectNamespace,
                Array.Empty<string>());
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
            return "List" + typeof(T).Name;
        }

        protected virtual CStyleFunction GenerateListFunction(CStyleParameter[] createFunctionParameters)
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

        private CStyleObject GenerateReturnObjectOnError()
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
                    new CStyleObjectInitalizer(options.ResponseObjectFieldName, "data")
                });
        }

        protected virtual CStyleStatement GenerateDatabaseMethod()
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@$"
                var sqlStatement = Db.From<{typeof(T).Name}>().Where(a=>a.{options.DbModelIdfield}  > request.{options.RequestObjectAfterField});
                  
                ");
            foreach(var field in options.SearchFields)
            {
                sb.AppendLine(@$"
                sqlStatement= sqlStatement.Where(a=>a.{field.Name}.Contains(request.{field.Name}));");
            }
            sb.Append($@"
        var data = await Db.SelectAsync(sqlStatement);
");
            //SearchFields
            return sb.ToString();
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
                "ServiceStack",
                typeof(T).Namespace
            };
            var extends = new
                CStyleClass(
                    $"IReturn<{options.ResponseObjectType}>", options.RequestObjectNamespace);

            var requestObjectFields = new List<CStyleClassField>(new[]
            {
                new CStyleClassField(options.RequestObjectAfterField,
                    new CStyleTypeDeclaration("int"))
            });
            foreach (var field in options.SearchFields)
            {
     
                requestObjectFields.Add(new CStyleClassField(field.Name, new CStyleTypeDeclaration(field.TypeName())));
            }
            return new CStyleClass(options.RequestObjectType,
                options.RequestObjectNamespace,
                usings: usings,
                extends: extends,
                fields: requestObjectFields.ToArray());
        }

        public virtual CStyleClass CreateResponseType()
        {
            var usings = new string[]
            {
                "System",
                "System.Collections.Generic"
            };
            var responseObjectFields = new[]
            {
                new CStyleClassField("Count",
                    new CStyleTypeDeclaration("long")),
                new CStyleClassField("Message",
                    new CStyleTypeDeclaration("string")),
                new CStyleClassField("Success",
                    new CStyleTypeDeclaration("bool")),
                new CStyleClassField(options.ResponseObjectFieldName,
                    new CStyleTypeDeclaration($"List<{typeof(T).Name}>")),
            };
            return new(options.ResponseObjectType, options.ResponseObjectNamespace, usings: usings, fields:
                responseObjectFields);
        }
    }
}