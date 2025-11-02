using AutoMapper;

namespace Niu.Nutri.Core.Application.DTO.Seedwork
{
    public static class MapperFactory
    {
        static Mapper? _mapper;
        
        static List<Profile?>? _profiles { get; set; }
        
        public static Mapper Mapper { get { return _mapper ?? throw new Exception("Mapper Not Initialized"); } }

        public static void Setup(string nmspc)
        {
            _profiles = _profiles ?? new List<Profile?>();

            var assmbs = AppDomain.CurrentDomain.GetType().Namespace;

            _profiles.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(p => p.GetTypes())
                            .Where(p => (p.BaseType == typeof(Profile) || p.BaseType?.BaseType == typeof(Profile)) && p.Namespace.Contains(nmspc))
                            .ToList()
                            .Select(x => Activator.CreateInstance(x) as Profile ?? throw new ArgumentNullException(nameof(x.Name))));


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                foreach (var profile in _profiles)
                {
                    try
                    {
                        cfg.AddProfile(profile);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            });

            _mapper = new Mapper(config);
        }
    }

}
