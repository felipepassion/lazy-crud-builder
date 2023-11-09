using System.Globalization;
using System.Text.RegularExpressions;

string oldValue         =       @"C:\Users\felip\source\repos\Equipe-LazyCrud\LazyCrudBuilder.NET\back-end\src\Addresses";
string newValue         =       @"C:\Users\felip\source\repos\Equipe-LazyCrud\LazyCrudBuilder.NET\back-end\src\TesteShow";
string solutionFile     =       @"C:\Users\felip\source\repos\Equipe-LazyCrud\LazyCrudBuilder.NET\back-end\LazyCrudBuilder.sln";

//Chama as funções para copiar pastas e arquivos e adicionar projetos ao arquivo de solução 
CopyFolderAndFiles(oldValue, newValue);
AddProjectsToSolution(newValue, solutionFile);

//Função que copia as pastas e arquivos dentro da pasta especificada, para o novo diretório
static void CopyFolderAndFiles(string oldValue, string newValue)
{
    var searchValue = oldValue.Split('\\').Last();
    var replaceValue = newValue.Split('\\').Last();

    // Copia pastas
    string[] directories = Directory.GetDirectories(oldValue, "*", SearchOption.AllDirectories);
    foreach (string directory in directories)
    {
        string newDirectory = directory.Replace(searchValue, replaceValue);
        if (!Directory.Exists(newDirectory))
        {
            Directory.CreateDirectory(newDirectory);
        }
    }

    // Copia arquivos
    string[] files = Directory.GetFiles(oldValue, "*.*", SearchOption.AllDirectories);
    foreach (string file in files)
    {
        if (file.Contains(@"\T4\")) continue;
        if (file.Contains(@"\Entities\")) continue;
        if (file.Contains(@"\Migrations\")) continue;
        if (file.Contains(@"\Enumerations\")) continue;
        if (file.Contains(".cs")) continue;

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
        // Obtém caminho relactive para o arquivo .csproj
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

static void RunT4(string file)
{
}