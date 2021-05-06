using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

class MainClass {

  static StreamReader Leer;
  static StreamWriter Escribir;
  static StreamWriter Escribir2;

  struct empleado{
    public string Nombre;
    public string Apellido;
    public string Cargo;
    public int Horastrabajo;
    public Double sueldoliquido;
    public Double sueldobase;
  }

  public static int login(){

    Leer = new StreamReader("login.txt", true);
    int cont = 0;
    string usuario;
    string contra;
    string usuariodef;
    string contradef;
    int estado;
    usuariodef = Leer.ReadLine();
    contradef = Leer.ReadLine();
    estado = int.Parse(Leer.ReadLine());
    Leer.Close();

    if(estado == 0){
      cont = 0;
      while(cont < 3){
        Console.Write("Usuario: ");
        usuario = Console.ReadLine();
        Console.Write("Contraseña: ");
        contra = Console.ReadLine();
        if(usuario != usuariodef || contra != contradef){
          Console.WriteLine("Intento fallido");
          if(cont == 2){
            Console.WriteLine("Usuario Bloqueado");
            Escribir = new StreamWriter("login.txt", false);
            Escribir.WriteLine("alex");
            Escribir.WriteLine("alex123");
            Escribir.WriteLine("1");
            Escribir.Close();
            return 1;
          }
        }else{
          Console.WriteLine("Inicio de sesion exitoso");
          return 0;
        }
        cont++;
      }
    }else{
      Console.WriteLine("Usuario Bloqueado");
      return 1;
    }
    return 2;
  }

  static void carga(empleado[] empleados){
    Console.WriteLine("Ingrese los datos de los empleados");
    for(int cont=0; cont<3; cont++){
      Console.WriteLine("Escriba sus nombres");
      empleados[cont].Nombre = Console.ReadLine();

      Console.WriteLine("Escriba sus apellidos");
      empleados[cont].Apellido = Console.ReadLine();

      Console.WriteLine("Escriba su cargo");
      empleados[cont].Cargo = Console.ReadLine();

      Console.WriteLine("Total de horas trabajadas durante el mes");
      empleados[cont].Horastrabajo = int.Parse(Console.ReadLine());
    }
  }

  static void deducciones(empleado[] empleados){
    string ganamas = "ganamas", ganamenos = "ganamenos";
    double bono = 0, excepcion = 0, isss = 0.0525, afp = 0.0688, renta = 0.1, mas300 = 0, mayorsalario = 0, menorsalario = 0;
    
    Directory.CreateDirectory(@"calculoEmple");
    DateTime fecha = DateTime.Now.ToLocalTime();
    string path = @"calculoEmple";
    string path2 = "calculoSalario_" + fecha.ToString("ddMMyyyy_HHmm") + ".txt";

    Escribir2 = new StreamWriter(Path.Combine(path, path2), true);

    for(int cont=0; cont<3; cont++){
      if(empleados[cont].Horastrabajo <= 0){
        continue;
      }

      if(empleados[cont].Cargo == "Gerente"){
        bono = 0.10;
        if(cont == 0){
          excepcion++;
        }
      }else{
        if(empleados[cont].Cargo == "Asistente"){
          bono = 0.05;
          if(cont == 1){
            excepcion++;
          }
        }else{
          if(empleados[cont].Cargo == "Secretaria"){
            bono = 0.03;
            if(cont == 2){
              excepcion++;
            }
          }else{
            bono = 0.02;
          }
        }
      }

      if(empleados[cont].Horastrabajo <= 160){
        empleados[cont].sueldobase = empleados[cont].Horastrabajo * 9.75;
      }else{
        empleados[cont].sueldobase = (160 * 9.75) + ((empleados[cont].Horastrabajo - 160) * 11.50);
      }

      empleados[cont].sueldoliquido = empleados[cont].sueldobase - ((empleados[cont].sueldobase * isss) + (empleados[cont].sueldobase * afp) + (empleados[cont].sueldobase * renta));

      if(empleados[cont].sueldoliquido > 300){
        mas300++;
      }

      if((empleados[cont].sueldoliquido + (empleados[cont].sueldoliquido * bono)) > mayorsalario){
        mayorsalario = (empleados[cont].sueldoliquido + (empleados[cont].sueldoliquido * bono));
        ganamas = empleados[cont].Nombre + " " + empleados[cont].Apellido;
      }

      if(menorsalario == 0){
        menorsalario = (empleados[cont].sueldoliquido + (empleados[cont].sueldoliquido * bono));
        ganamenos = empleados[cont].Nombre + " " + empleados[cont].Apellido;
      }else{
        if((empleados[cont].sueldoliquido + (empleados[cont].sueldoliquido * bono)) < menorsalario){
          menorsalario = (empleados[cont].sueldoliquido + (empleados[cont].sueldoliquido * bono));
          ganamenos = empleados[cont].Nombre + " " + empleados[cont].Apellido;
        }
      }
    }
    for(int cont=0; cont<3; cont++){
      if(empleados[cont].Cargo == "Gerente"){
        bono = 0.10;
      }else{
        if(empleados[cont].Cargo == "Asistente"){
          bono = 0.05;
        }else{
          if(empleados[cont].Cargo == "Secretaria"){
            bono = 0.03;
          }else{
            bono = 0.02;
          }
        }
      }

      Console.WriteLine("Nombre del empleado: " + empleados[cont].Nombre + " " + empleados[cont].Apellido);
      Escribir2.WriteLine("Nombre del empleado: " + empleados[cont].Nombre + " " + empleados[cont].Apellido);

      Console.WriteLine("Descuentos");
      Escribir2.WriteLine("Descuentos");

      Console.WriteLine("ISSS = " + (isss*100) + "%");
      Escribir2.WriteLine("ISSS = " + (isss*100) + "%");

      Console.WriteLine("AFP = " + (afp*100) + "%");
      Escribir2.WriteLine("AFP = " + (afp*100) + "%");

      Console.WriteLine("RENTA = " + (renta*100) + "%");
      Escribir2.WriteLine("RENTA = " + (renta*100) + "%");

      Console.WriteLine("Sueldo líquido a pagar = $" + empleados[cont].sueldoliquido);
      Escribir2.WriteLine("Sueldo líquido a pagar = $" + empleados[cont].sueldoliquido);
      
      if(excepcion == 3){
        Console.WriteLine("NO HAY BONO");
        Escribir2.WriteLine("NO HAY BONO");
      }else{
        Console.WriteLine("Si es apto para bonos = $" + (empleados[cont].sueldoliquido + (empleados[cont].sueldoliquido * bono)));
        Escribir2.WriteLine("Si es apto para bonos = $" + (empleados[cont].sueldoliquido + (empleados[cont].sueldoliquido * bono)));
      }

      Console.WriteLine();
      Escribir2.WriteLine();
    }
    Console.WriteLine("Empleado con mayor salario: " + ganamas);
    Escribir2.WriteLine("Empleado con mayor salario: " + ganamas);
    Console.WriteLine("Empleado con menor salario: " + ganamenos);
    Escribir2.WriteLine("Empleado con menor salario: " + ganamenos);
    Console.WriteLine("Numero de personas que ganan más de $300 = " + mas300);
    Escribir2.WriteLine("Numero de personas que ganan más de $300 = " + mas300);
    Escribir2.Close();

    /* Directory.CreateDirectory(@"hist");
    string path3 = @"hist";
    string path4 = "calculoSalario_" + fecha.ToString("ddMMyyyy_HHmm") + ".zip";
    ZipFile.CreateFromDirectory(Path.Combine(path, path2), Path.Combine(path3, path4));
    */
  }

  public static void Main (string[] args) {
    int estadologin = 0;

    Console.WriteLine ("Inicio de sesion");
    estadologin = login();

    if(estadologin == 0){
      empleado[] empleados = new empleado[3];
      carga(empleados);
      deducciones(empleados);
    }
  }
}