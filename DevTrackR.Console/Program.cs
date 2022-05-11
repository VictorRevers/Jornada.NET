// See https://aka.ms/new-console-template for more information
var pacotes = new List<Pacote>();

Console.WriteLine("----- DevTrackR - Serviço de Postagem -----");

ExibirMensagemPrincipal();

var opcao = Console.ReadLine();

while(opcao != "0")
{
    switch(opcao)
    {
        case "1":
            CadastrarPacote();            
            break;
        case "2":
            AtualizarPacote();
            Console.WriteLine("Pacote Atualizado!");
            break;
        case "3":
            ConsultarPacote();           
            break;
            default:
                Console.WriteLine("Opção Inválida!");
                break;
    }
    ExibirMensagemPrincipal();
    opcao = Console.ReadLine();

}
 
void ExibirMensagemPrincipal()
{
    Console.WriteLine("Digite o código de acordo com o que você quer: ");
    Console.WriteLine("1- Cadastro de Pacote");
    Console.WriteLine("2- Atualizar Pacote");
    Console.WriteLine("3- Consultar Pacote");
    Console.WriteLine("0- Sair");
}

void CadastrarPacote()
{
    Console.WriteLine("Titulo: ");
    var titulo = Console.ReadLine();

    Console.WriteLine("Descricao: ");
    var descricao = Console.ReadLine();


    var pacote = new Pacote(titulo, descricao);

    pacotes.Add(pacote);
    Console.WriteLine($"O Pacode de código {pacote.Codigo} foi cadastrado com sucesso!");
}

void AtualizarPacote()
{
    Console.WriteLine("Digite o código do pacote: ");
    var codigo = Console.ReadLine();

    var pacote = pacotes.SingleOrDefault(p => p.Codigo == codigo);

    if(pacote == null)
    {
        Console.WriteLine("Pacote não encontrado!");
        return;
    }else
    {
        Console.WriteLine("Novo Status: ");
        var status = Console.ReadLine();

        pacote.AtualizarStatus(status);
    }
}

void ConsultarPacote()
{
    Console.WriteLine("Digite o código do pacote: ");
    var codigo = Console.ReadLine();

     var pacote = pacotes.SingleOrDefault(p => p.Codigo == codigo);

    if(pacote == null)
    {
        Console.WriteLine("Pacote não encontrado!");
        return;
    }else
    {
        pacote.ExibirDetalhes();
    }
}

var pacotePremium = new PacotePremium("Pacote premium", "Um pacote premium", "RJ582");
var pacoteComum = new Pacote("Pacote comum", "pacote comum");

var conjuntPacotes = new List<Pacote> {pacotePremium, pacoteComum};

foreach(var item in conjuntPacotes)
{
    item.ExibirDetalhes();
}
public class Pacote
{
    public Pacote(string titulo, string descricao)
    {
        Titulo = titulo;
        Descricao = descricao;
        Codigo = GerarCodigo();
        DataPostagem = DateTime.Now;
        Status = "Postado.";
    }

    private string GerarCodigo()
    {
        return Guid.NewGuid().ToString();
    }

    public void AtualizarStatus(string status)
    {
        Status = status;
    }

    public virtual void ExibirDetalhes()
    {
        Console.WriteLine($"Pacote {Titulo}, Código: {Codigo}, Status: {Status} ");
    }


    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Codigo { get; set; }
    public DateTime DataPostagem {get; set;}
    public string Status { get; set; } 
}

public class PacotePremium : Pacote
{
    public PacotePremium(string titulo, string descricao, string voo) 
    : base(titulo, descricao)
    {
        Voo = voo;
    } 
    

    public string Voo {get; set;}
    public override void ExibirDetalhes() 
    {
        base.ExibirDetalhes();
        Console.WriteLine($"Voo: {Voo}");
    }
}

