using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Commands.Responses;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;

namespace LazyCrud.Core.Application.DTO.Extensions.Http
{
    public static class PageInitializator
    {
        public async static Task<T> OnInitializedAsync<T>(this HttpClient Http, string id)
            where T : EntityDTO, new()
        {
            try
            {
                try
                {
                    var testResult = await Http.GetAsync($"{new T().GetRoute()}?ExternalIdEqual={id}");
                    var content = await testResult.Content.ReadAsStringAsync();
                    var test = JsonConvert.DeserializeObject<GetHttpResponseDTO<T>>(await testResult.Content.ReadAsStringAsync());
                    return test.Data;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                //ex.Redirect();
            }
            return null;
        }

        public async static Task<T> FindOneAsync<T>(this HttpClient Http, string query)
            where T : EntityDTO, new()
        {
            try
            {
                var testResult = await Http.GetAsync($"{new T().GetRoute()}?{query}");
                var content = await testResult.Content.ReadAsStringAsync();
                var test = JsonConvert.DeserializeObject<GetHttpResponseDTO<T>>(await testResult.Content.ReadAsStringAsync());
                return test.Data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async static Task<List<T>> SearchAsync<T>(this HttpClient Http, string route, string query = null)
            where T : EntityDTO, new()
        {
            try
            {
                var url = $"{new T().GetRoute()}{(route.Contains('/') ? "" : @"/")}";
                return (await Http.GetFromJsonAsync<GetHttpResponseDTO<List<T>>>(
                    $"{url}{route}?{query}"))?.Data;
            }
            catch (Exception ex)
            {
                //ex.Redirect();
            }
            return null;
        }

        public async static Task<List<T>> SearchAsync<T>(this HttpClient Http, string query)
            where T : EntityDTO, new()
        {
            try
            {
                return (await Http.GetFromJsonAsync<GetHttpResponseDTO<List<T>>>($"{new T().GetSearchRoute()}?{query}"))?.Data;
            }
            catch (Exception ex)
            {
                //ex.Redirect();
            }
            return null;
        }

        public async static Task<K[]> SelectAsync<T, K>(this HttpClient Http)
            where T : EntityDTO, new()
            where K : EntityDTO, new()
        {
            try
            {
                return (await Http.GetFromJsonAsync<GetHttpResponseDTO<K[]>>($"{new T().GetSelectRoute()}"))?.Data;
            }
            catch (Exception ex)
            {
                //ex.Redirect();
            }
            return null;
        }

        public async static Task<K[]> SummaryAsync<T, K>(this HttpClient Http, string query = null)
            where T : EntityDTO, new()
            where K : EntityDTO, new()
        {
            try
            {
                return (await Http.GetFromJsonAsync<GetHttpResponseDTO<K[]>>($"{new T().GetSummaryRoute()}?{query}"))?.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async static Task<K[]> SummaryAsync<T, K>(this HttpClient Http, Expression<Func<T, bool>> expressions)
            where T : EntityDTO, new()
            where K : EntityDTO, new()
        {
            try
            {
                var query = LambdaToFilterQueryString(expressions);
                return (await Http.GetFromJsonAsync<GetHttpResponseDTO<K[]>>($"{new T().GetSummaryRoute()}?{query}"))?.Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async static Task<int?> CountAsync<T>(this HttpClient Http, string query = null)
            where T : EntityDTO, new()
        {
            try
            {
                return int.TryParse((await Http.GetFromJsonAsync<GetHttpResponseDTO>($"{new T().CountRoute()}?{query}"))?.Data.ToString(), out var val)
                    ? val : throw new Exception($"Não foi possível converter a respostar do {nameof(CountAsync)}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async static Task<GetHttpResponseDTO<DomainResponseDTO>> CreateAsync<T>(this HttpClient Http, T model)
            where T : IEntityDTO, new()
        {
            try
            {
                var converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                };

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = converters
                };

                string jsonContent = JsonConvert.SerializeObject(model, settings);
                HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var result = await Http.PostAsync(new T().GetRoute(), httpContent);
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<GetHttpResponseDTO<DomainResponseDTO>>(content, settings);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(content ?? ex.Message);
                    }
                    throw new Exception(await result.Content.ReadAsStringAsync());
                }

                return JsonConvert.DeserializeObject<GetHttpResponseDTO<DomainResponseDTO>>(content, settings);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public static async Task<GetHttpResponseDTO<DomainResponseDTO>> DeleteAsync<T>(this HttpClient http, Expression<Func<T, bool>> query)
     where T : IEntityDTO, new()
        {
            try
            {
                var test = query.LambdaToQueryString();
                var result = await http.DeleteAsync($"{new T().GetDeleteRoute()}?{test}");

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(await result.Content.ReadAsStringAsync());
                }

                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<GetHttpResponseDTO<DomainResponseDTO>>(content);

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async static Task<GetHttpResponseDTO> UploadImage(this HttpClient httpClient, string fileNameAndPath, byte[] image)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/images/", new ImageFileInfoDTO { Image = image, Name = fileNameAndPath });

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to upload file: {response.ReasonPhrase}");
                }

                var result = JsonConvert.DeserializeObject<GetHttpResponseDTO>(await response.Content.ReadAsStringAsync());
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public async static Task<GetHttpResponseDTO> UploadFile(this HttpClient httpClient, string fileNameAndPath, byte[] image)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/files/", new ImageFileInfoDTO { Image = image, Name = fileNameAndPath });

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to upload file: {response.ReasonPhrase}");
                }

                var result = JsonConvert.DeserializeObject<GetHttpResponseDTO>(await response.Content.ReadAsStringAsync());
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public static string LambdaToFilterQueryString<T>(Expression<Func<T, bool>> expression)
        {
            var replacements = new Dictionary<string, string>();
            WalkExpression(replacements, expression);

            string body = expression.Body.ToString();

            foreach (var parm in expression.Parameters)
            {
                var parmName = parm.Name;
                var parmTypeName = parm.Type.Name;
                body = body.Replace(parmName + ".", parmTypeName + ".");
            }

            foreach (var replacement in replacements)
            {
                body = body.Replace(replacement.Key, replacement.Value);
            }

            return body;
        }

        private static void WalkExpression(Dictionary<string, string> replacements, Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    string replacementExpression = expression.ToString();
                    if (replacementExpression.Contains("value("))
                    {
                        string replacementValue = Expression.Lambda(expression).Compile().DynamicInvoke().ToString();
                        if (!replacements.ContainsKey(replacementExpression))
                        {
                            replacements.Add(replacementExpression, replacementValue);
                        }
                    }
                    break;

                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:
                case ExpressionType.Equal:
                    var bexp = expression as BinaryExpression;
                    WalkExpression(replacements, bexp.Left);
                    WalkExpression(replacements, bexp.Right);
                    break;

                case ExpressionType.Call:
                    var mcexp = expression as MethodCallExpression;
                    foreach (var argument in mcexp.Arguments)
                    {
                        WalkExpression(replacements, argument);
                    }
                    break;

                case ExpressionType.Lambda:
                    var lexp = expression as LambdaExpression;
                    WalkExpression(replacements, lexp.Body);
                    break;

                case ExpressionType.Constant:
                    // Do nothing
                    break;

                default:
                    Trace.WriteLine("Unknown type");
                    break;
            }
        }
    }
}
