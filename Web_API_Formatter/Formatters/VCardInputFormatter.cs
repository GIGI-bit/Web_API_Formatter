using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using Microsoft.Net.Http.Headers;
using System.Text;
using Web_API_Formatter.DTOs;


namespace WbApiDemo3_22_5.Formatters
{
    public class VCardInputFormatter : TextInputFormatter
    {
        public VCardInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        => type == typeof(StudentAddDTO);

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;

            using var reader = new StreamReader(httpContext.Request.Body, encoding);
            string? nameLine = null;

            try
            {
                //Id - Fullname - SeriaNo - Age - Score
                nameLine = await ReadLineAsync( reader, context);
                var split = nameLine.Split("-".ToCharArray());
                var student = new StudentAddDTO
                {

                    Fullname = split[1].Trim(),
                    SeriaNo = split[2].Trim(),
                    Age = Int32.Parse(split[3].Trim()),
                    Score = Int32.Parse(split[1].Trim()),
                  };
              

                return await InputFormatterResult.SuccessAsync(student);
            }
            catch (Exception e)
            {
                return await InputFormatterResult.FailureAsync();

            }



        }

        private static async Task<string> ReadLineAsync(
     StreamReader reader, InputFormatterContext context, string expectedText = "")
        {
            var line = await reader.ReadLineAsync();

            if (line is null || !line.StartsWith(expectedText))
            {
                var errorMessage = $"Looked for '{expectedText}' and got '{line}'";

                context.ModelState.TryAddModelError(context.ModelName, errorMessage);

                throw new Exception(errorMessage);
            }

            return line;
        }



    }

}
