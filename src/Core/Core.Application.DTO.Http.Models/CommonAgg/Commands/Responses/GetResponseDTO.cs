namespace Niu.Nutri.Core.Application.DTO.Http.Models.CommonAgg.Commands.Responses
{
    public class GetDTO : GetDTO<object>
    {

    }
    public class GetDTO<T>
    {
        public GetDTO() { }

        public GetDTO(T response)
        {
            Data = response;
        }

        public virtual bool Success
        {
            get { return Errors?.Any() != true; }
        }

      

        public void AddError(params string[] newErrors)
        {
            var list = Errors?.ToList() ?? new List<string>();
            list.AddRange(newErrors);
            Errors = list.ToArray();
        }

        public string[] Errors { get; set; } = [];
        public T Data { get; set; }
    }
}
