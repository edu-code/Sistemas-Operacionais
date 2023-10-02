// No problema do jantar dos filosofos, crie um protocolo que permita uma operacao sem impasse.

using System.Diagnostics;

namespace JantarDosFilosofos
{
 class Program
    {
        private const int FilosofoCount = 5;

        static void Main(string[] args)
        {
            var filosofos = InicializarFilosofos();
            
            Console.WriteLine("O jantar está começando.");
            
            var filosofoThreads = new List<Thread>();
            foreach (var filosofo in filosofos)
            {
                var filosofoThread = new Thread(filosofo.ComerAll);
                filosofoThreads.Add(filosofoThread);
                filosofoThread.Start();
            }
            
            foreach (var thread in filosofoThreads)
            {
                thread.Join();
            }

            // Done
            Console.WriteLine("O jantar acabou.");
        }

        private static List<Filosofo> InicializarFilosofos()
        {
            // Construct filosofos
            var filosofos = new List<Filosofo>(FilosofoCount);
            for (int i = 0; i < FilosofoCount; i++)
            {
                filosofos.Add(new Filosofo(filosofos, i));
            }
            
            foreach (var filosofo in filosofos)
            {
                filosofo.GarfoEsquerdo = filosofo.FilosofoEsquerdo.GarfoDireito;
                
                filosofo.GarfoDireito = filosofo.FilosofoDireito.GarfoEsquerdo;
            }

            return filosofos;
        }
    }

    [DebuggerDisplay("Nome = {Nome}")]
    public class Filosofo
    {
        private const int VezesParaComer = 5;
        private int _vezesQueComeu;
        private readonly List<Filosofo> _allfilosofos;
        private readonly int _index;

        public Filosofo(List<Filosofo> allfilosofos, int index)
        {
            _allfilosofos = allfilosofos;
            _index = index;
            Nome = string.Format("filosofo {0}", _index);
            Estado = Estado.Pensando;
        }

        public string Nome { get; private set; }
        public Estado Estado { get; private set; }
        public Garfo GarfoEsquerdo { get; set; }
        public Garfo GarfoDireito { get; set; }

        public Filosofo FilosofoEsquerdo
        {
            get
            {
                if (_index == 0)
                    return _allfilosofos[_allfilosofos.Count - 1];
                return _allfilosofos[_index - 1];
            }
        }

        public Filosofo FilosofoDireito
        {
            get
            {
                if (_index == _allfilosofos.Count - 1)
                    return _allfilosofos[0];
                return _allfilosofos[_index + 1];
            }
        }

        public void ComerAll()
        {
            while (_vezesQueComeu < VezesParaComer)
            {
                Pensar();
                if (PegarGarfo())
                {
                    Comer();
                    
                    SoltarGarfoEsquerdo();
                    SoltarGarfoDireito();
                }
            }
        }

        private bool PegarGarfo()
        {
            if (Monitor.TryEnter(GarfoEsquerdo))
            {
                Console.WriteLine(Nome + " pega o garfo esquerdo.");
                
                if (Monitor.TryEnter(GarfoDireito))
                {
                    Console.WriteLine(Nome + " pega o garfo direito.");
                    
                    return true;
                }

                SoltarGarfoEsquerdo();
            }
            
            return false;
        }

        private void Comer()
        {
            Estado = Estado.Comendo;
            _vezesQueComeu++;
            Console.WriteLine(Nome + " come.");
         }

        private void SoltarGarfoEsquerdo()
        {
            Monitor.Exit(GarfoEsquerdo);
            Console.WriteLine(Nome + " solta o garfo esquerdo.");
        }

        private void SoltarGarfoDireito()
        {
            Monitor.Exit(GarfoDireito);
            Console.WriteLine(Nome + " solta o garfo direito.");
        }
        
        private void Pensar()
        {
            Estado = Estado.Pensando;
        }
    }

    public enum Estado
    {
        Pensando = 0,
        Comendo = 1
    }

    [DebuggerDisplay("Nome = {Nome}")]
    public class Garfo
    {
        private static int _count = 1;
        public string Nome { get; private set; }

        public Garfo()
        {
            Nome = "Garfo " + _count++;
        }
    }
}