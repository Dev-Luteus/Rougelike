using UnityEngine;

/* Create an instance of MyClass >
 * > Then pass in myClass (object) as parameter, of type IMyInterface:  TestInterface<my IMyInterface>(myClass)
 * TestInterface(myClass) uses: myInterface.TestFunction() >
 * > Which calls void TestFunction(); in interface IMyInterface
 */
public class InterfacePalette
{
    private void Start()
    {
        // Test interface wants an Object of IMyInterface
        // Test interface expects an Implementation of (IMyInterface)
        // MyClass implements interface (Inheritance)
        MyClass myClass = new();
        TestInterface(myClass);
    }
    private void TestInterface(IMyInterface myInterface)
    {
        myInterface.TestFunction();
    }
}
public interface IMyInterface
{
    void TestFunction();
}
public class MyClass : IMyInterface
{
    public void TestFunction()
    {
        Debug.Log("MyClass.TestFunction()");
    }
} 
