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

                await ReadLineAsync("BEGIN:VCARD", reader, context);
                nameLine = await ReadLineAsync("FN:", reader, context);
                var split = nameLine.Split(":".ToCharArray());
                var student = new StudentAddDTO
                {
                    Fullname = split[0] + split[1],
                };
                nameLine = await ReadLineAsync("SNO:", reader, context);
                split = nameLine.Split(":".ToCharArray());
                student.SeriaNo = split[0];
                nameLine = await ReadLineAsync("AGE:", reader, context);
                split = nameLine.Split(":".ToCharArray());
                student.Age = Int32.Parse(split[1]);
                nameLine = await ReadLineAsync("SCORE:", reader, context);
                split = nameLine.Split(":".ToCharArray());
                student.Score = Int32.Parse(split[1]);
                await ReadLineAsync("END:VCARD", reader, context);

                return await InputFormatterResult.SuccessAsync(student);

            }
            catch (Exception e)
            {
                return await InputFormatterResult.FailureAsync();

            }



        }

        private static async Task<string> ReadLineAsync(
    string expectedText, StreamReader reader, InputFormatterContext context)
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