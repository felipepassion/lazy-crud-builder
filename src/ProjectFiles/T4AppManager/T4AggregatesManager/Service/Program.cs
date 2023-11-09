using LazyCrudBuilder.CrossCuting.Infra.Utils.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;

public static class AggregateCreator
{
    static string oldValue = @"..\..\..\DefaultTemplate";
    static string solutionFile = @"C:\Users\felip\source\repos\Equipe-LazyCrud\LazyCrudBuilder.NET\back-end\LazyCrudBuilder.sln";

    //Função que copia as pastas e arquivos dentro da pasta especificada, para o novo diretório
    public static void DeleteAggregate(string newValue)
    {
        newValue = $@"..\..\..\{newValue}";
        Directory.Delete(newValue, true);
    }

    public static void DeleteEntity(string aggName, string entityName)
    {
        aggName = $@"..\..\..\{aggName}\{aggName}.Domain\Aggregates\{aggName}Agg\Entities\{entityName}.cs";
        File.Delete(aggName);
    }

    public static void CreateEntity(string aggName, string entityName)
    {
        var copyFromTemplate = File.ReadAllLines($@"..\..\..\DefaultTemplate\DefaultTemplate.Domain\Aggregates\DefaultTemplateAgg\Entities\DefaultEntity.cs");
        for (int i = 0; i < copyFromTemplate.Length; i++)
        {
            copyFromTemplate[i] = copyFromTemplate[i].Replace("DefaultTemplateAgg", aggName).Replace("DefaultEntity", ToTitleCase(entityName)).Replace("DefaultTemplate", aggName);
        }
        var fileName = $@"..\..\..\{aggName}\{aggName}.Domain\Aggregates\{aggName}Agg\Entities\{ToTitleCase(entityName)}.cs";
        File.WriteAllLines(fileName, copyFromTemplate);
    }

    public static void CreateNewAggregate(string newValue)
    {
        newValue = $@"..\..\..\{newValue}";

        var searchValue = oldValue.Split('\\').Last();
        var replaceValue = newValue.Split('\\').Last();

        // Copia pastas
        string[] directories = Directory.GetDirectories(oldValue, "*", SearchOption.AllDirectories);
        foreach (string directory in directories)
        {

            string newDirectory = directory.Replace(searchValue, replaceValue);

            if (directory.Equals(@"bin")) continue;
            if (directory.Equals(@"obj")) continue;

            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }
        }

        // Copia arquivos
        string[] files = Directory.GetFiles(oldValue, "*.*", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (file.Contains(@"\bin\")) continue;
            if (file.Contains(@"\obj\")) continue;
            if (file.Contains(@"\T4\")) continue;
            if (file.Contains(@"\Entities\")) continue;
            if (file.Contains(@"\Migrations\")) continue;
            if (file.Contains(@"\Enumerations\")) continue;
            if (file.EndsWith(".cs")) continue;

            string newFile = file.Replace(searchValue, replaceValue);
            if (File.Exists(newFile))
            {
                File.Delete(newFile);
            }
            File.Copy(file, newFile);

            // Procurando e substituindo as ocorrências da palavra no arquivo de destino, preservando o "case" da palavra original
            string fileContent = File.ReadAllText(file);
            fileContent = Regex.Replace(fileContent, searchValue, match =>
            {
                return ReplaceByCase(match, replaceValue);
            }, RegexOptions.IgnoreCase);
            File.WriteAllText(newFile, fileContent);

            if (newFile.Contains(".tt")) RunT4(newFile);
        }

        #region MyRegion

        static string ReplaceByCase(Match match, string replaceValue)
        {
            if (match.Value.All(char.IsUpper))
                return replaceValue.ToUpper();
            else if (match.Value.All(char.IsLower))
                return replaceValue.ToLower();
            else if (IsCamelCase(match.Value))
                return ToCamelCase(replaceValue);
            else if (IsPascalCase(match.Value))
                return ToPascalCase(replaceValue);
            else if (IsSnakeCase(match.Value))
                return ToSnakeCase(replaceValue);
            else if (IsKebabCase(match.Value))
                return ToKebabCase(replaceValue);
            else
                return ToTitleCase(replaceValue);
        }

        static bool IsCamelCase(string input)
        {
            // Verifica se a string está no formato Camel Case
            // (primeira letra minúscula, seguida de letras maiúsculas ou minúsculas)
            // Exemplo: "camelCase"
            return char.IsLower(input[0]) && input.Skip(1).All(char.IsLetter);
        }

        static bool IsPascalCase(string input)
        {
            // Verifica se a string está no formato Pascal Case
            // (primeira letra maiúscula, seguida de letras maiúsculas ou minúsculas)
            // Exemplo: "PascalCase"
            return char.IsUpper(input[0]) && input.Skip(1).All(char.IsLetter);
        }

        static bool IsSnakeCase(string input)
        {
            // Verifica se a string está no formato Snake Case
            // (letras minúsculas separadas por sublinhados)
            // Exemplo: "snake_case"
            return input.All(c => char.IsLower(c) || c == '_');
        }

        static bool IsKebabCase(string input)
        {
            // Verifica se a string está no formato Kebab Case
            // (letras minúsculas separadas por traços)
            // Exemplo: "kebab-case"
            return input.All(c => char.IsLower(c) || c == '-');
        }

        static string ToCamelCase(string input)
        {
            // Converte a string para Camel Case
            // (primeira letra minúscula, seguida de letras maiúsculas)
            // Exemplo: "camelCase"
            return char.ToLower(input[0]) + input.Substring(1);
        }

        static string ToSnakeCase(string input)
        {
            // Converte a string para Snake Case
            // (letras minúsculas separadas por sublinhados)
            // Exemplo: "snake_case"
            return input.Replace(" ", "_");
        }

        static string ToKebabCase(string input)
        {
            // Converte a string para Kebab Case
            // (letras minúsculas separadas por traços)
            // Exemplo: "kebab-case"
            return input.Replace(" ", "-");
        }
        static string ToPascalCase(string input)
        {
            // Converte a string para Pascal Case
            // (primeira letra maiúscula, seguida de letras maiúsculas)
            // Exemplo: "PascalCase"
            return char.ToUpper(input[0]) + input.Substring(1);
        }
        #endregion
    }

    static string ToTitleCase(string s)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
    }

    static void AddProjectsToSolution(string path, string solutionFile)
    {
        // Obtém lista de arquivos .csproj
        string[] csprojFiles = Directory.GetFiles(path, "*.csproj");

        // Abre o arquivo .sln para adicionar novos projetos
        string solutionContent = File.ReadAllText(solutionFile);

        // Adiciona novos projetos ao arquivo .sln
        foreach (string csprojFile in csprojFiles)
        {
            // Obtém caminho relativo para o arquivo .csproj
            string csprojPath = Path.GetRelativePath(path, csprojFile);

            // Obtém o nome do projeto a partir do arquivo .csproj
            string csprojContent = File.ReadAllText(csprojFile);
            Match projectNameMatch = Regex.Match(csprojContent, @"<AssemblyName>(.+)</AssemblyName>");
            string projectName = projectNameMatch.Groups[1].Value;

            // Adiciona projeto ao arquivo .sln
            solutionContent += $@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{projectName}"", ""{csprojPath}"", ""{{{Guid.NewGuid()}}}""
EndProject
";
        }

        // Salva alterações no arquivo .sln
        File.WriteAllText(solutionFile, solutionContent);
    }

    public static void AdicionarPropriedadeSeNaoExistir(string caminhoArquivo, string tipo, string nomePropriedade)
    {
        // Lê o conteúdo do arquivo .cs
        string codigoFonte = File.ReadAllText(caminhoArquivo);

        // Define o padrão para encontrar a classe no arquivo
        string padraoClasse = @"class\s+([a-zA-Z_]\w*)\s*[:\s\w]*\{";

        // Procura pela classe no código-fonte
        Match classeMatch = Regex.Match(codigoFonte, padraoClasse);

        if (classeMatch.Success)
        {
            // Obtém o nome da classe
            string nomeClasse = classeMatch.Groups[1].Value;

            // Verifica se a propriedade já existe
            string padraoPropriedade = $@"public\s+{tipo}\s+{nomePropriedade}\s*{{\s*get;\s*set;\s*}}";
            if (!Regex.IsMatch(codigoFonte, padraoPropriedade))
            {
                // A propriedade não existe, então a adicionamos
                string novaPropriedade = $"public {tipo} {nomePropriedade} {{ get; set; }}";

                // Encontre o índice de fechamento da classe
                int indiceFechamentoClasse1 = codigoFonte.LastIndexOf('}');
                int indiceFechamentoClasse = codigoFonte.Substring(0, indiceFechamentoClasse1).LastIndexOf('}');

                if (indiceFechamentoClasse >= 0)
                {
                    // Obtém a quebra de linha adequada para a plataforma
                    string quebraDeLinha = Environment.NewLine;

                    // Insira a nova propriedade logo antes do fechamento da classe
                    string novoCodigoFonte = codigoFonte.Insert(indiceFechamentoClasse, $"{quebraDeLinha}        {novaPropriedade}{quebraDeLinha}    ");

                    // Grava o código-fonte atualizado de volta no arquivo
                    File.WriteAllText(caminhoArquivo, novoCodigoFonte);
                }
            }
            else
            {
                Console.WriteLine("A propriedade já existe no arquivo.");
            }
        }
        else
        {
            Console.WriteLine("Classe não encontrada no arquivo.");
        }
    }


    public static void CanDeletePropriedadeComAtributos(string caminhoArquivo, string nomePropriedade)
    {
        try
        {
            // Lê o conteúdo do arquivo .cs
            string[] linhas = File.ReadAllLines(caminhoArquivo);

            // Define o padrão de expressão regular para encontrar a propriedade
            string padraoPropriedade = $@"public\s+([\w?<>]+)\s+{nomePropriedade}\s*{{";

            // Encontra a linha onde a propriedade está localizada
            int linhaPropriedade = -1;
            for (int i = 0; i < linhas.Length; i++)
            {
                if (Regex.IsMatch(linhas[i], padraoPropriedade))
                {
                    linhas[i] = "{REMOVED}";
                    linhaPropriedade = i;
                    break;
                }
            }

            if (linhaPropriedade != -1)
            {
                // Remove a propriedade do código-fonte
                for (int i = linhaPropriedade - 1; i >= 0; i--)
                {
                    // Remove manualmente linhas de atributos acima da propriedade
                    if (linhas[i].Contains("[") && linhas[i].Contains("]"))
                    {
                        linhas[i] = "{REMOVED}";
                    }
                    else
                    {
                        break; // Paramos quando não há mais atributos
                    }
                }

                // Grava o código-fonte atualizado de volta no arquivo
                File.WriteAllLines(caminhoArquivo, linhas.Where(x => !x.Equals("{REMOVED}")));

                Console.WriteLine($"A propriedade '{nomePropriedade}' e seus atributos foram removidos com sucesso!");
            }
            else
            {
                Console.WriteLine($"A propriedade '{nomePropriedade}' não foi encontrada no arquivo.");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ocorreu um erro ao ler/gravar o arquivo: {ex.Message}");
        }
    }

    public static void UpdatePropriedadeComAtributos(string caminhoArquivo, string nomePropriedade, string type, string name = null)
    {
        try
        {
            // Lê o conteúdo do arquivo .cs
            string[] linhas = File.ReadAllLines(caminhoArquivo);

            // Define o padrão de expressão regular para encontrar a propriedade
            string padraoPropriedade = $@"public\s+([\w?<>[\]]+)\s+{nomePropriedade}\s*{{";

            // Encontra a linha onde a propriedade está localizada
            int linhaPropriedade = -1;
            for (int i = 0; i < linhas.Length; i++)
            {
                if (Regex.IsMatch(linhas[i], padraoPropriedade))
                {
                    string linhaOriginal = linhas[i];

                    // Verificando e removendo espaços em branco no início da linha, mantendo a identação.
                    int espacosIniciais = linhaOriginal.Length - linhaOriginal.TrimStart().Length;
                    string linhaSemIdentacao = linhaOriginal.Trim();

                    // Realize suas operações aqui na `linhaSemIdentacao` (por exemplo, o seu código atual).
                    // Por exemplo, vamos supor que você queira realizar as alterações que mencionou:
                    var split = linhaSemIdentacao.Split(" ");
                    split[1] = type;
                    split[2] = string.IsNullOrWhiteSpace(name) ? split[2] : name;
                    string linhaComAlteracoes = string.Join(" ", split);

                    // Reconstituindo a linha com a identação original.
                    string linhaFinal = new string(' ', espacosIniciais) + linhaComAlteracoes;

                    // Substituindo a linha original pela linha final no array.
                    linhas[i] = linhaFinal;

                    // Grava o código-fonte atualizado de volta no arquivo

                    Console.WriteLine($"A propriedade '{nomePropriedade}' e seus atributos foi alterada com sucesso!");
                }
                File.WriteAllLines(caminhoArquivo, linhas);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ocorreu um erro ao ler/gravar o arquivo: {ex.Message}");
        }
    }

    public static Dictionary<string, string> GetProperties(string caminhoArquivo)
    {
        // Lê o conteúdo do arquivo .cs
        string codigoFonte = File.ReadAllText(caminhoArquivo);

        // Define o padrão para encontrar a classe no arquivo
        string padraoClasse = @"class\s+([a-zA-Z_]\w*)\s*[:\s\w]*\{";

        // Procura pela classe no código-fonte
        Match classeMatch = Regex.Match(codigoFonte, padraoClasse);
        Dictionary<string, string> propriedades = new Dictionary<string, string>();

        if (classeMatch.Success)
        {
            // Obtém o nome da classe
            string nomeClasse = classeMatch.Groups[1].Value;

            // Inicialize uma lista para armazenar as linhas que contêm "{ get; set; }"
            List<string> linhasPropriedades = new List<string>();

            // Separe o código-fonte em linhas
            string[] linhas = codigoFonte.Split('\n');

            // Itere pelas linhas do código-fonte
            foreach (string linha in linhas)
            {
                // Verifique se a linha contém "{ get; set; }"
                if (linha.Replace(" ", "").Contains("{get;set;}"))
                {
                    propriedades.Add(linha.Trim().Split(" ")[2], linha.Trim().Split(" ")[1]);
                }
            }

            return propriedades;
        }
        else
        {
            throw new Exception("Classe não encontrada no arquivo. - " + caminhoArquivo);
        }
    }

    static void RunT4(string file)
    {
    }
}