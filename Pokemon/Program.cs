using Newtonsoft.Json;
using Pokemon;
using System.Net;
using System;
using System.Drawing;
using System.IO;
using static System.Net.WebRequestMethods;

static async Task<Bicho> BuscaBicho()
{
    using var client = new HttpClient();
    var randomId = new Random().Next(1, 151);
    var url = $"https://pokeapi.co/api/v2/pokemon/{randomId}";
    var response = await client.GetAsync(url);

    var json = await response.Content.ReadAsStringAsync();

    return JsonConvert.DeserializeObject<Bicho>(json);



}

static void DownloadImagemPokemon(string imagemUrl, string nomePokemon)
{
    //construindo o diretorio pra salvar a imagem
    string pastaImagens = Path.Combine(Directory.GetCurrentDirectory(), "imagens");

    // nome do arquivo
    string caminho = Path.Combine(pastaImagens, $"{nomePokemon}.jpg");

    if(!Directory.Exists(pastaImagens))
    {
        Directory.CreateDirectory(pastaImagens);
    }

    using (WebClient client = new WebClient())
    {
        client.DownloadFile(imagemUrl, caminho);
    }

}

static Bitmap ResizeImage(Bitmap image, int width, int height)
{
    Bitmap resizedImage = new Bitmap(width, height);
    using (Graphics g = Graphics.FromImage(resizedImage))
    {
        g.DrawImage(image, 0, 0, width, height);
    }
    return resizedImage;

}

static char[,] ConvertToAscii(Bitmap image)
{
    char[] asciiChars = " .:-=+*#%@".ToCharArray(); // Tabela de mapeamento exemplo

    int width = image.Width;
    int height = image.Height;
    char[,] asciiImage = new char[height, width];

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            Color pixelColor = image.GetPixel(x, y);
            int grayScale = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
            int index = (int)(grayScale / 255.0 * (asciiChars.Length - 1));
            asciiImage[y, x] = asciiChars[index];
        }
    }

    return asciiImage;
}

static void ImagemParaAscii(string nomePokemon)
{
    string pastaImagens = Path.Combine(Directory.GetCurrentDirectory(), "imagens");

    string caminhoArquivo = Path.Combine(pastaImagens, $"{nomePokemon}.jpg");

    int width = 90;
    int height = 45;
    // Carrega a imagem
    Bitmap image = new Bitmap(caminhoArquivo);
    image = ResizeImage(image, width, height);

    char[,] asciiImage = ConvertToAscii(image);

    // Imprimir a imagem ASCII no console
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            Console.Write(asciiImage[y, x]);
        }
        Console.WriteLine();
    }
}

var pokemon = await BuscaBicho();
DownloadImagemPokemon(pokemon.Sprites.Front_default, pokemon.Name);
Console.WriteLine(pokemon.Name);
var chute = 0;
Console.WriteLine("Quem é esse pokemon, quem é? chuta um aí!");
while (true)
{
    Console.WriteLine("");
    Console.WriteLine("Qual é o pokemon que eu achei?!");
    var resposta = Console.ReadLine().ToLower();
    chute++;
    if (resposta == pokemon.Name.ToLower())
    {
        Console.WriteLine($"Acertou!! É o {pokemon.Name.ToUpper()}");
        break;
    } 
    else if (chute == 1)
    {
        Console.WriteLine($"Errado! Olha a primeira dica: ");
        Console.WriteLine($"O Pokemon é do tipo {pokemon.Types[0].Type.Name}");
    }
    else if(chute == 2)
    {
        Console.WriteLine($"Errado!! Pega essa dica:");
        Console.WriteLine($"O número na Pokedex é {pokemon.Id}");
    }
    else
    {
        Console.WriteLine("Errado! Agora não tem como errar: ");
        ImagemParaAscii(pokemon.Name);
    }

}