using Magazin;
using System.Net;
using Telegram.Bot;

class Program
{
    public static void CheckCategorii()
    {
        string folderPathCategorii = "../../../../";
        string fileNameCategorii = "Categorii.txt";
        string filePathCategoriiTxt = Path.Combine(folderPathCategorii, fileNameCategorii);

        if (!File.Exists(filePathCategoriiTxt))
        {
            using (File.Create(filePathCategoriiTxt)) { }
        }

        string fileNameProduse = "Produse.txt";
        string filePathProduseTxt = Path.Combine(folderPathCategorii, fileNameProduse);

        if (!File.Exists(filePathProduseTxt))
        {
            using (File.Create(filePathProduseTxt)) { }
        }

        List<string> categorii = File.ReadAllLines(filePathCategoriiTxt).ToList();

        List<string> categoriiFolosite = File.ReadLines(filePathProduseTxt)
                                          .Select(line => line.Split(',')[1].Trim())
                                          .Distinct()
                                          .ToList();

        List<string> unusedCategories = categorii.Except(categoriiFolosite).ToList();

        if (unusedCategories.Count > 0)
        {
            List<string> remainingCategories = categorii.Except(unusedCategories).ToList();
            File.WriteAllLines(filePathCategoriiTxt, remainingCategories);
        }
    }

    public static int NextCod()
    {
        string folderPathProduse = "../../../../";
        string fileNameProduse = "Produse.txt";
        string filePathProduseTxt = Path.Combine(folderPathProduse, fileNameProduse);

        if (!File.Exists(filePathProduseTxt))
        {
            using (File.Create(filePathProduseTxt)) { }
        }

        string[] linesProdus = File.ReadAllLines(filePathProduseTxt);
        if (linesProdus.Length == 0)
        {
            return 0;
        }
        else
        {
            int maxCod = linesProdus
                .Select(lineProdus => int.Parse(lineProdus.Split(',')[0]))
                .Max();
            return maxCod + 1;
        }
    }

    private static string GetIPAddress()
    {
        string ipAddress = string.Empty;
        System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (System.Net.IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
                break;
            }
        }
        return ipAddress;
    }

    private static string GetMachineName()
    {
        return Environment.MachineName;
    }

    private static string GetOSVersion()
    {
        return Environment.OSVersion.VersionString;
    }

    public static bool Login()
    {
        string folderPath = "../../../../";

        string fileNameLog = "login.txt";
        string filePathLogTxt = Path.Combine(folderPath, fileNameLog);

        string fileNamePass = "password.txt";
        string filePathPassTxt = Path.Combine(folderPath, fileNamePass);

        string botToken = "5900642493:AAGbGw9kHg_8XVO4ZThs01CWpXILM9rwpIs";
        string chatId = "-1001760055401";

        var botClient = new TelegramBotClient(botToken);

        string storedLogin = File.ReadAllText(filePathLogTxt);
        string storedPassword = File.ReadAllText(filePathPassTxt);
        Console.WriteLine("\n================== CrossDrive ==================\n");
        Console.Write("LOGIN: ");
        string enteredLogin = Console.ReadLine();

        Console.Write("\nPASSWORD: ");
        string enteredPassword = Console.ReadLine();

        if (enteredLogin != storedLogin || enteredPassword != storedPassword)
        {
            string ipAddress = GetIPAddress();
            string machineName = GetMachineName();
            string osVersion = GetOSVersion();
            DateTime date = DateTime.Now;

            string message = $"Încercare nereușită - CrossDrive\nData: {date}\nIP: {ipAddress}\nNumele utilizatorului: {machineName}\nOS: {osVersion}";

            botClient.SendTextMessageAsync(chatId, message).Wait();

            return false;
        }
        return true;
    }

    static void Main(string[] args)
    {
        if (!Login())
        {
            return;
        }

        if (args is null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        List<Produs> listProduse = new();
        List<Client> listClienti = new();

    MENIU:

        while (true)
        {
            Program.CheckCategorii();

            Console.Clear();
            Console.WriteLine("\n================== MENIU ==================\n");
            Console.WriteLine("1. PRODUSE\n");
            Console.WriteLine("2. CLIENTI\n");
            Console.WriteLine("0. EXIT");
            Console.Write("\nAlegeti o optiune: ");

            string folderPathProduse = "../../../../";
            string fileNameProduse = "Produse.txt";
            string filePathProduseTxt = Path.Combine(folderPathProduse, fileNameProduse);

            string menuOpt = Console.ReadLine();

            switch (menuOpt)
            {
                case "1":

                PRODUS:

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("\n================== MENIU PRODUSE ==================\n");
                        Console.WriteLine("1. Adaugare produs\n");
                        Console.WriteLine("2. Afisare produse\n");
                        Console.WriteLine("3. Cautare produs\n");
                        Console.WriteLine("4. Editare produs\n");
                        Console.WriteLine("5. Sterge produs\n");
                        Console.WriteLine("0. Inapoi");
                        Console.Write("\nAlegeti o optiune: ");

                        string produsOpt = Console.ReadLine();

                        switch (produsOpt)
                        {
                            case "1":
                                Console.Clear();
                                Console.WriteLine("\n================== ADAUGARE PRODUS ==================\n");

                                int codProdus = NextCod();

                                Console.Write("Introduceti categoria produsului: ");
                                string catProdus = Console.ReadLine();

                                string folderPathCategorii = "../../../../";
                                string fileNameCategorii = "Categorii.txt";
                                string filePathCategoriiTxt = Path.Combine(folderPathCategorii, fileNameCategorii);

                                if (!File.Exists(filePathCategoriiTxt))
                                {
                                    using (File.Create(filePathCategoriiTxt)) { }
                                }

                                string catProdusNewLine = catProdus + Environment.NewLine;

                                string[] catProdusAllreadyExists = File.ReadAllLines(filePathCategoriiTxt);

                                bool catProdusExists = Array.Exists(catProdusAllreadyExists, line => line.Equals(catProdus));

                                if (!catProdusExists)
                                {
                                    File.AppendAllText(filePathCategoriiTxt, catProdusNewLine);
                                }

                                Console.Write("Introduceti denumirea produsului: ");
                                string denumireProdus = Console.ReadLine();

                                Console.Write("Introduceti pretul produsului: ");
                                double pretProdus = double.Parse(Console.ReadLine());

                                Produs produs = new(codProdus, catProdus, denumireProdus, pretProdus);
                                listProduse.Add(produs);

                                using (StreamWriter writer = new(filePathProduseTxt, true))
                                {
                                    writer.WriteLine($"{codProdus},{catProdus},{denumireProdus},{pretProdus}");
                                }

                                Console.WriteLine("\nProdusul a fost salvat in Produse.txt cu codul {0}.\n", codProdus);
                                Console.WriteLine("Apasati orice tasta pentru a continua...");
                                Console.ReadKey();
                                break;

                            case "2":
                                Console.Clear();
                                Console.WriteLine("\n================== AFISARE TOATE PRODUSELE ==================\n");

                                if (!File.Exists(filePathProduseTxt))
                                {
                                    using (File.Create(filePathProduseTxt)) { }
                                }

                                string[] linesAfisareProdus = File.ReadAllLines(filePathProduseTxt);
                                bool findProduse = false;

                                Console.WriteLine("{0,-7} {1,-20} {2,-70} {3,-5}", "Cod", "Categorie", "Denumire", "Pret");
                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------");

                                foreach (string lineAfisareProdus in linesAfisareProdus)
                                {
                                    string[] valuesProdus = lineAfisareProdus.Split(',');

                                    if (valuesProdus.Length >= 4)
                                    {
                                        string cod_produs = valuesProdus[0];
                                        string categorie_produs = valuesProdus[1];
                                        string denumire_produs = valuesProdus[2];
                                        string pret_produs = valuesProdus[3];

                                        Console.WriteLine("{0,-7} {1,-20} {2,-70} {3,-5}", cod_produs, categorie_produs, denumire_produs, pret_produs);
                                        findProduse = true;
                                    }
                                }

                                if (!findProduse)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\n================== AFISARE TOATE PRODUSELE ==================\n");
                                    Console.WriteLine("Nu s-au gasit produse in lista.");
                                }

                                Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                Console.ReadKey();
                                break;

                            case "3":
                                Console.Clear();
                                Console.WriteLine("\n================== CAUTARE PRODUS ==================\n");

                                Console.Write("Introduceti codul produsului: ");
                                string codProdusCautat = Console.ReadLine();

                                Console.Clear();
                                Console.WriteLine("\n================== PRODUS ID: {0} ==================\n", codProdusCautat);

                                if (!File.Exists(filePathProduseTxt))
                                {
                                    using (File.Create(filePathProduseTxt)) { }
                                }

                                string[] linesCautareProdus = File.ReadAllLines(filePathProduseTxt);
                                bool findCautareProduse = false;
                                foreach (string lineProdus in linesCautareProdus)
                                {
                                    string[] valuesProdus = lineProdus.Split(',');
                                    if (valuesProdus[0] == codProdusCautat)
                                    {
                                        Console.WriteLine("Categorie: {0}\nDenumire: {1}\nPret: {2} lei", valuesProdus[1], valuesProdus[2], valuesProdus[3]);
                                        findCautareProduse = true;
                                        break;
                                    }
                                }

                                if (!findCautareProduse)
                                {
                                    Console.WriteLine("Nu sa gasit niciun produs cu acest cod");
                                    Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("\nApasati orice tasta pentru a continua...");
                                    Console.ReadKey();
                                }
                                break;

                            case "4":
                                Console.Clear();
                                Console.WriteLine("\n================== EDITARE PRODUS ==================\n");

                                Console.Write("Introduceti codul produsului care va fi editat: ");
                                string codProdusEditat = Console.ReadLine();

                                if (!File.Exists(filePathProduseTxt))
                                {
                                    using (File.Create(filePathProduseTxt)) { }
                                }

                                string[] linesProdusEditat = File.ReadAllLines(filePathProduseTxt);
                                bool findProdusEditat = false;

                                for (int i = 0; i < linesProdusEditat.Length; i++)
                                {
                                    string[] valuesProdusEditat = linesProdusEditat[i].Split(',');
                                    if (valuesProdusEditat[0] == codProdusEditat)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n================== EDITARE PRODUS ID: {0} ==================\n", codProdusEditat);
                                        Console.WriteLine("1. Categorie: {0}", valuesProdusEditat[1]);
                                        Console.WriteLine("2. Denumire: {0}", valuesProdusEditat[2]);
                                        Console.WriteLine("3. Pret: {0} lei", valuesProdusEditat[3]);
                                        Console.WriteLine("0. Anulare");
                                        Console.Write("\nIntroduceti optiunea care doriti sa o editati (sau '0' pentru a anula editarea): ");

                                        string produsEditOpt = Console.ReadLine();

                                        switch (produsEditOpt)
                                        {
                                            case "1":
                                                Console.Write("\nIntroduceti noua categorie a produsului: ");
                                                string newCategorie = Console.ReadLine();

                                                string folderPathEditCategorii = "../../../../";
                                                string fileNameEditCategorii = "Categorii.txt";
                                                string filePathEditCategoriiTxt = Path.Combine(folderPathEditCategorii, fileNameEditCategorii);

                                                string catProdusEditNewLine = newCategorie + Environment.NewLine;

                                                string[] catProdusEditAllreadyExists = File.ReadAllLines(filePathEditCategoriiTxt);

                                                bool catProdusEditExists = Array.Exists(catProdusEditAllreadyExists, line => line.Equals(newCategorie));

                                                if (!catProdusEditExists)
                                                {
                                                    File.AppendAllText(filePathEditCategoriiTxt, catProdusEditNewLine);
                                                }

                                                linesProdusEditat[i] = $"{valuesProdusEditat[0]},{newCategorie},{valuesProdusEditat[2]},{valuesProdusEditat[3]}";
                                                findProdusEditat = true;
                                                break;

                                            case "2":
                                                Console.Write("\nIntroduceti noua denumire a produsului: ");
                                                string newDenumire = Console.ReadLine();
                                                linesProdusEditat[i] = $"{valuesProdusEditat[0]},{valuesProdusEditat[1]},{newDenumire},{valuesProdusEditat[3]}";
                                                findProdusEditat = true;
                                                break;

                                            case "3":
                                                Console.Write("\n\nIntroduceti noul pret al produsului: ");
                                                double newPret = double.Parse(Console.ReadLine());
                                                linesProdusEditat[i] = $"{valuesProdusEditat[0]},{valuesProdusEditat[1]},{valuesProdusEditat[2]},{newPret}";
                                                findProdusEditat = true;
                                                break;

                                            case "0":
                                                goto PRODUS;

                                            default:
                                                Console.WriteLine("\nOptiunea introdusa nu exista.");
                                                Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                                Console.ReadKey();
                                                goto PRODUS;
                                        }
                                    }
                                }

                                if (findProdusEditat)
                                {
                                    File.WriteAllLines(filePathProduseTxt, linesProdusEditat);
                                    Console.WriteLine("\nProdusul a fost editat cu succes.");
                                }
                                else
                                {
                                    Console.WriteLine("\nNu s-a gasit niciun produs cu acest cod.");
                                    Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                    Console.ReadKey();
                                }
                                break;

                            case "5":
                                Console.Clear();
                                Console.WriteLine("\n================== STERGE PRODUS ==================\n");

                                Console.Write("Introduceti codul produsului care va fi sters: ");
                                string codStergeProdus = Console.ReadLine();

                                if (!File.Exists(filePathProduseTxt))
                                {
                                    using (File.Create(filePathProduseTxt)) { }
                                }

                                string[] linesStergeProdus = File.ReadAllLines(filePathProduseTxt);
                                bool findStergeProdus = false;

                                for (int i = 0; i < linesStergeProdus.Length; i++)
                                {
                                    string[] valuesStergeProdus = linesStergeProdus[i].Split(',');
                                    if (valuesStergeProdus[0] == codStergeProdus)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n================== STERGE PRODUS ID: {0} ==================\n", codStergeProdus);
                                        Console.WriteLine("Categorie: {0}", valuesStergeProdus[1]);
                                        Console.WriteLine("Denumire: {0}", valuesStergeProdus[2]);
                                        Console.WriteLine("Pret: {0} lei", valuesStergeProdus[3]);
                                        Console.WriteLine("\nSunteti sigur ca doriti sa stergeti acest produs?");
                                        Console.WriteLine("\n1. DA!");
                                        Console.WriteLine("\n0. NU!");
                                        Console.Write("\nOptiunea aleasa: ");

                                        string produsDeleteOpt = Console.ReadLine();

                                        switch (produsDeleteOpt)
                                        {
                                            case "1":
                                                linesStergeProdus = linesStergeProdus.Where((line, index) => index != i).ToArray();
                                                findStergeProdus = true;
                                                Console.WriteLine("\nProdusul a fost sters cu succes.");
                                                break;

                                            case "0":
                                                goto PRODUS;

                                            default:
                                                Console.WriteLine("\nOptiunea introdusa nu exista.");
                                                Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                                Console.ReadKey();
                                                goto PRODUS;
                                        }
                                    }
                                }

                                if (findStergeProdus)
                                {
                                    File.WriteAllLines(filePathProduseTxt, linesStergeProdus);
                                    Console.WriteLine("\nProdusul a fost sters cu succes.");
                                }
                                else
                                {
                                    Console.WriteLine("\nNu s-a gasit niciun produs cu acest cod.");
                                    Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                    Console.ReadKey();
                                }
                                break;

                            case "0":
                                goto MENIU;

                            default:
                                Console.WriteLine("\nOptiunea introdusa nu exista.\n");
                                Console.WriteLine("Apasati orice tasta pentru a incerca din nou...");
                                Console.ReadKey();
                                break;
                        }
                    }

                case "2":

                CLIENT:

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("\n================== MENIU CLIENTI ==================\n");
                        Console.WriteLine("1. Adaugare client\n");
                        Console.WriteLine("2. Afisare clienti\n");
                        Console.WriteLine("3. Cautare client\n");
                        Console.WriteLine("4. Editare client\n");
                        Console.WriteLine("5. Sterge client\n");
                        Console.WriteLine("0. Inapoi");
                        Console.Write("\nAlegeti o optiune: ");

                        string folderPathClienti = "../../../../";
                        string fileNameClienti = "Clienti.txt";
                        string filePathClientiTxt = Path.Combine(folderPathClienti, fileNameClienti);

                        if (!File.Exists(filePathClientiTxt))
                        {
                            using (File.Create(filePathClientiTxt)) { }
                        }

                        string clientOpt = Console.ReadLine();

                        switch (clientOpt)
                        {
                            case "1":
                                Console.Clear();
                                Console.WriteLine("\n================== ADAUGARE CLIENT ==================\n");

                                Console.Write("Introduceti codul personal al clientului: ");
                                string codPersonal = Console.ReadLine();

                                bool ClientExists(string codPersonal)
                                {
                                    string[] lines = File.ReadAllLines(filePathClientiTxt);
                                    foreach (string line in lines)
                                    {
                                        string[] values = line.Split(',');
                                        if (values.Length >= 1 && values[0] == codPersonal)
                                        {
                                            return true;
                                        }
                                    }
                                    return false;
                                }

                                if (ClientExists(codPersonal))
                                {
                                    Console.WriteLine("\nClientul cu acest cod personal ({0}) exista deja in fisierul Clienti.txt.", codPersonal);
                                    Console.WriteLine("\nApasati orice tasta pentru a reveni in meniu...");
                                    Console.ReadKey();
                                    break;
                                }

                                Console.Write("Introduceti numele clientului: ");
                                string nume = Console.ReadLine();

                                Console.Write("Introduceti prenumele clientului: ");
                                string prenume = Console.ReadLine();

                                Console.Write("Introduceti suma cheltuita de client: ");
                                double suma = double.Parse(Console.ReadLine());

                                Client client = new(codPersonal, nume, prenume, suma);
                                listClienti.Add(client);
                                using (StreamWriter writer = new(filePathClientiTxt, true))
                                {
                                    writer.WriteLine($"{codPersonal},{nume},{prenume},{suma}");
                                }

                                Console.WriteLine("\nClientul a fost salvat in Clienti.txt cu codul personal {0}.\n", codPersonal);
                                Console.WriteLine("Apasati orice tasta pentru a continua...");
                                Console.ReadKey();
                                break;

                            case "2":
                                Console.Clear();
                                Console.WriteLine("\n================== AFISARE TOTI CLIENTII ==================\n");

                                if (!File.Exists(filePathClientiTxt))
                                {
                                    using (File.Create(filePathClientiTxt)) { }
                                }

                                string[] linesAfisareClient = File.ReadAllLines(filePathClientiTxt);
                                bool findAfisareClient = false;

                                Console.WriteLine("{0,-20} {1,-15} {2,-15} {3,-10}", "Cod personal", "Nume", "Prenume", "Suma cheltuita");
                                Console.WriteLine("-----------------------------------------------------------------------");

                                foreach (string lineAfisareClient in linesAfisareClient)
                                {
                                    string[] valuesClient = lineAfisareClient.Split(',');

                                    if (valuesClient.Length >= 4)
                                    {
                                        string cod_client = valuesClient[0];
                                        string nume_client = valuesClient[1];
                                        string prenume_client = valuesClient[2];
                                        string suma_client = valuesClient[3];

                                        Console.WriteLine("{0,-20} {1,-15} {2,-15} {3,-10}", cod_client, nume_client, prenume_client, suma_client);
                                        findAfisareClient = true;
                                    }
                                }

                                if (!findAfisareClient)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\n================== AFISARE TOTI CLIENTII ==================\n");
                                    Console.WriteLine("Nu s-au gasit clienti in lista.");
                                }

                                Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                Console.ReadKey();
                                break;

                            case "3":
                                Console.Clear();
                                Console.WriteLine("\n================== CAUTARE CLIENT ==================\n");

                                Console.Write("Introduceti codul personal al clientului: ");
                                string codPersonalCautat = Console.ReadLine();

                                Console.Clear();
                                Console.WriteLine("\n================== CLIENT ID: {0} ==================", codPersonalCautat);

                                if (!File.Exists(filePathClientiTxt))
                                {
                                    using (File.Create(filePathClientiTxt)) { }
                                }

                                string[] linesCautareClient = File.ReadAllLines(filePathClientiTxt);
                                bool findCautareClient = false;
                                foreach (string lineClient in linesCautareClient)
                                {
                                    string[] valuesClient = lineClient.Split(',');
                                    if (valuesClient[0] == codPersonalCautat)
                                    {
                                        Console.WriteLine("\nCod personal: {0}\nNume si prenume: {1} {2}\nSuma cheltuita: {3} lei", valuesClient[0], valuesClient[1], valuesClient[2], valuesClient[3]);
                                        findCautareClient = true;
                                        break;
                                    }
                                }

                                if (!findCautareClient)
                                {
                                    Console.WriteLine("\nNu sa gasit niciun client cu acest cod personal");
                                    Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("\nApasati orice tasta pentru a continua...");
                                    Console.ReadKey();
                                }
                                break;

                            case "4":
                                Console.Clear();
                                Console.WriteLine("\n================== EDITARE CLIENT ==================\n");

                                Console.Write("Introduceti codul personal al clientului care va fi editat: ");
                                string codPersonalEditat = Console.ReadLine();

                                if (!File.Exists(filePathClientiTxt))
                                {
                                    using (File.Create(filePathClientiTxt)) { }
                                }

                                string[] linesClientEditat = File.ReadAllLines(filePathClientiTxt);
                                bool findClientEditat = false;
                                for (int i = 0; i < linesClientEditat.Length; i++)
                                {
                                    string[] valuesClientEditat = linesClientEditat[i].Split(',');
                                    if (valuesClientEditat[0] == codPersonalEditat)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n================== EDITARE CLIENT ID: {0} ==================\n", codPersonalEditat);
                                        Console.WriteLine("1. Cod Personal: {0}", valuesClientEditat[0]);
                                        Console.WriteLine("2. Nume: {0}", valuesClientEditat[1]);
                                        Console.WriteLine("3. Prenume: {0}", valuesClientEditat[2]);
                                        Console.WriteLine("4. Suma cheltuita: {0} lei", valuesClientEditat[3]);
                                        Console.WriteLine("0. Anulare");
                                        Console.Write("\nIntroduceti optiunea care doriti sa o editati (sau '0' pentru a anula editarea): ");

                                        string clientEditOpt = Console.ReadLine();

                                        switch (clientEditOpt)
                                        {
                                            case "1":
                                                Console.Write("\nIntroduceti noul cod personal al clientului: ");
                                                string newCodPersonal = Console.ReadLine();

                                                bool EditClientExists(string newCodPersonal)
                                                {
                                                    string[] lines = File.ReadAllLines(filePathClientiTxt);
                                                    foreach (string line in lines)
                                                    {
                                                        string[] values = line.Split(',');
                                                        if (values.Length >= 1 && values[0] == newCodPersonal)
                                                        {
                                                            return true;
                                                        }
                                                    }
                                                    return false;
                                                }

                                                if (EditClientExists(newCodPersonal))
                                                {
                                                    Console.WriteLine("\nClientul cu acest cod personal ({0}) exista deja in fisierul Clienti.txt.", newCodPersonal);
                                                    Console.WriteLine("\nApasati orice tasta pentru a reveni in meniu...");
                                                    Console.ReadKey();
                                                    goto CLIENT;
                                                }

                                                linesClientEditat[i] = $"{newCodPersonal},{valuesClientEditat[1]},{valuesClientEditat[2]},{valuesClientEditat[3]}";
                                                findClientEditat = true;
                                                break;

                                            case "2":
                                                Console.Write("\nIntroduceti noul nume al clientului: ");
                                                string newNume = Console.ReadLine();
                                                linesClientEditat[i] = $"{valuesClientEditat[0]},{newNume},{valuesClientEditat[2]},{valuesClientEditat[3]}";
                                                findClientEditat = true;
                                                break;

                                            case "3":
                                                Console.Write("\nIntroduceti noul prenume al clientului: ");
                                                string newPrenume = Console.ReadLine();
                                                linesClientEditat[i] = $"{valuesClientEditat[0]},{valuesClientEditat[1]},{newPrenume},{valuesClientEditat[3]}";
                                                findClientEditat = true;
                                                break;

                                            case "4":
                                                Console.Write("\nIntroduceti noua suma achitata de client: ");
                                                double newSuma = double.Parse(Console.ReadLine());
                                                linesClientEditat[i] = $"{valuesClientEditat[0]},{valuesClientEditat[1]},{valuesClientEditat[2]},{newSuma}";
                                                findClientEditat = true;
                                                break;

                                            case "0":
                                                goto CLIENT;

                                            default:
                                                Console.WriteLine("\nOptiunea introdusa nu exista.\n");
                                                Console.WriteLine("Apasati orice tasta pentru a reveni la meniu...");
                                                Console.ReadKey();
                                                goto CLIENT;
                                        }
                                    }
                                }

                                if (findClientEditat)
                                {
                                    File.WriteAllLines(filePathClientiTxt, linesClientEditat);
                                    Console.WriteLine("\nClientul a fost editat cu succes.");
                                }
                                else
                                {
                                    Console.WriteLine("\nNu s-a gasit niciun client cu acest cod personal.");
                                    Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                    Console.ReadKey();
                                }
                                break;

                            case "5":
                                Console.Clear();
                                Console.WriteLine("\n================== STERGE CLIENT ==================\n");

                                Console.Write("Introduceti codul personal al clientului care va fi sters: ");
                                string codPersonalStergeClient = Console.ReadLine();

                                if (!File.Exists(filePathClientiTxt))
                                {
                                    using (File.Create(filePathClientiTxt)) { }
                                }

                                string[] linesStergeClient = File.ReadAllLines(filePathClientiTxt);
                                bool findStergeClient = false;
                                for (int i = 0; i < linesStergeClient.Length; i++)
                                {
                                    string[] valuesStergeClient = linesStergeClient[i].Split(',');
                                    if (valuesStergeClient[0] == codPersonalStergeClient)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\n================== EDITARE CLIENT ID: {0} ==================\n", codPersonalStergeClient);
                                        Console.WriteLine("1. Cod Personal: {0}", valuesStergeClient[0]);
                                        Console.WriteLine("2. Nume: {0}", valuesStergeClient[1]);
                                        Console.WriteLine("3. Prenume: {0}", valuesStergeClient[2]);
                                        Console.WriteLine("4. Suma cheltuita: {0} lei", valuesStergeClient[3]);
                                        Console.WriteLine("\nSunteti sigur ca doriti sa stergeti acest client?");
                                        Console.WriteLine("\n1. DA!");
                                        Console.WriteLine("\n0. NU!");
                                        Console.Write("\nOptiunea aleasa: ");


                                        string clientEditOpt = Console.ReadLine();

                                        switch (clientEditOpt)
                                        {
                                            case "1":
                                                linesStergeClient = linesStergeClient.Where((line, index) => index != i).ToArray();
                                                findStergeClient = true;
                                                Console.WriteLine("\nProdusul a fost sters cu succes.");
                                                break;

                                            case "0":
                                                goto CLIENT;

                                            default:
                                                Console.WriteLine("\nOptiunea introdusa nu exista.\n");
                                                Console.WriteLine("Apasati orice tasta pentru a reveni la meniu...");
                                                Console.ReadKey();
                                                goto CLIENT;
                                        }
                                    }
                                }

                                if (findStergeClient)
                                {
                                    File.WriteAllLines(filePathClientiTxt, linesStergeClient);
                                    Console.WriteLine("\nClientul a fost editat cu succes.");
                                }
                                else
                                {
                                    Console.WriteLine("\nNu s-a gasit niciun client cu acest cod personal.");
                                    Console.WriteLine("\nApasati orice tasta pentru a reveni la meniu...");
                                    Console.ReadKey();
                                }
                                break;

                            case "0":
                                goto MENIU;

                            default:
                                Console.WriteLine("\nOptiunea introdusa nu exista.\n");
                                Console.WriteLine("Apasati orice tasta pentru a continua...");
                                Console.ReadKey();
                                break;
                        }
                    }

                case "0":
                    Console.WriteLine("\nProgramul a fost inchis.");
                    return;

                default:
                    Console.WriteLine("\nOptiunea introdusa nu exista.\n");
                    Console.WriteLine("Apasati orice tasta pentru a continua...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}