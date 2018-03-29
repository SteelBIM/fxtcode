using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
     public class Employee
    {
         public Employee(string name, decimal salary) 
         {
             this.Name = name;
             this.Salary = salary;
         }

         //工资
         public decimal Salary { get; set; }
         //姓名
         public string Name { get; set; }

         public override string ToString()
         {
             return string.Format("{0} ， {1:c}",Name,Salary);
         }

         public static bool CompareSalary(Employee e1, Employee e2) 
         {
             return e1.Salary < e2.Salary;
         }
    }

     public class BubbleSorter 
     {
         public static void sort<T>(IList<T> sortArray,Func<T,T,bool> comparison) 
         {
             bool swapped = true;
             do
             {
                 swapped = false;
                 for (int i = 0; i < sortArray.Count-1; i++)
                 {
                     if (comparison(sortArray[i+1],sortArray[i]))
                     {
                         T temp = sortArray[i];
                         sortArray[i] = sortArray[i + 1];
                         sortArray[i + 1] = temp;
                         swapped = true;
                     }
                     
                 }
             } while (swapped);


         }
     }
}
