using AutoMapper;

namespace Lazy.Crud.Builder.Application.DTO.Seedwork
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
                            .SelectMany(p => { try { return p.GetTypes(); } catch (System.Reflection.ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null)!; } })
                            .Where(p => p != null && (p.BaseType == typeof(Profile) || p.BaseType?.BaseType == typeof(Profile)) && p.Namespace != null && p.Namespace.Contains(nmspc))
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

        public static void Setup(params List<Profile> _profiles)
        {
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
