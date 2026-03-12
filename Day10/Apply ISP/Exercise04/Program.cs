// See https://aka.ms/new-console-template for more information

/*
Refactor into segregated interfaces:
1. IPrinter - Print()
2. IScanner - Scan()
3. IFax - Fax()
4. ICopier - Copy()
5. IEmailSender - SendEmail()

Implementations:
1. MultiFunctionPrinter - all interfaces
2. BasicPrinter - only IPrinter
3. Scanner - only IScanner
4. SmartPrinter - IPrinter, IScanner, ICopier, IEmailSender (no fax)
*/

IPrinter basicPrinter = new BasicPrinter();
IScanner scanner = new Scanner();
SmartPrinter smartPrinter = new SmartPrinter();
MultiFunctionPrinter multiFunctionPrinter = new MultiFunctionPrinter();

System.Console.WriteLine("Testing Basic Printer");
basicPrinter.Print();
System.Console.WriteLine();

System.Console.WriteLine("Testing the Scanner");
scanner.Scan();
System.Console.WriteLine();

System.Console.WriteLine("Testing Smart Printer");
smartPrinter.Print();
smartPrinter.Copy();
smartPrinter.Scan();
smartPrinter.SendEmail();
System.Console.WriteLine();

System.Console.WriteLine(("multiFunctionPrinter Ttesting"));
multiFunctionPrinter.Print();
multiFunctionPrinter.Scan();
multiFunctionPrinter.Fax();
multiFunctionPrinter.Copy();
multiFunctionPrinter.SendEmail();
public interface IPrinter
{
    void Print();
}
public interface IScanner
{
    void Scan();
}
public interface IFax
{
    void Fax();
}
public interface ICopier
{
    void Copy();
}
public interface IEmailSender
{
    void SendEmail();
}

public class MultiFunctionPrinter : IPrinter, IScanner, IFax, ICopier, IEmailSender
{
    public void Print()
    {
        System.Console.WriteLine("Printing......");
    }

    public void Scan()
    {
        System.Console.WriteLine("Scanning the docs....");
    }

    public void Fax()
    {
        System.Console.WriteLine("Sending the Fax....");
    }
    public void Copy()
    {
        System.Console.WriteLine("Printing the multiple copies....");
    }
    public void SendEmail()
    {
        System.Console.WriteLine("Sending the Email....");
    }

}
public class BasicPrinter : IPrinter
{
    public void Print()
    {
        System.Console.WriteLine("Printing......");
    }
}

public class Scanner : IScanner
{
    public void Scan()
    {
        System.Console.WriteLine("Scanning the docs....");
    }
}
public class SmartPrinter : IScanner, IPrinter, ICopier, IEmailSender
{
    public void Print()
    {
        System.Console.WriteLine("Printing......");
    }

    public void Scan()
    {
        System.Console.WriteLine("Scanning the docs....");
    }

    public void Copy()
    {
        System.Console.WriteLine("Printing the multiple copies....");
    }
    public void SendEmail()
    {
        System.Console.WriteLine("Sending the Email....");
    }

}

