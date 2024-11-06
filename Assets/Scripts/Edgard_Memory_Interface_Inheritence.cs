using System;
using UnityEngine;

public class EdgarInterfaceExample
{
    
}
// OLD SCHOOL INHERITENCE BELOW
public abstract class Character // internal ? 
{
    public string Name { get; set; }
    public string Race { get; set; }
    public int Health { get; set; }

    public void Talk(string message)
    {
        Console.WriteLine("test");
    }
}
public class Player : Character // In Theory, Player still has Name, Race, Health
{
    public int Mana { get; set; }
}
class Enemy : Character
{
    public new int Health { get; set; } // New, does NOT inherit from Character. Its a NEW Value type
}
class NPC : Character
{
    public int Mana { get; set; }
}
class Program
{
    static void Main()
    {
        int x = 0;        // Local > Value Type: Goes in the Stack
        Player player = new(); // Instantiation : Goes in the Heap
        
        Console.WriteLine("Enter player name");
        player.Name = Console.ReadLine();
        
        player.Mana = 100; // is not declared, but it is part of Somewhere
        player.Talk("sss"); // not in player, but in Character
    }
}

/* This is example Below is bad. Because name race health is repeated TWICE
 * DRY : Don't Repeat Yourself
 * Make an ABSTRACT base class Character. 
 * Player, Enemy, NPC inherits from Character.
public class Player
{
    public string Name { get; set; }
    public string Race { get; set; }
    public int Health { get; set; }
}
public class Enemy
{
    public string Name { get; set; }
    public string Race { get; set; }
    public int Health { get; set; }
}*/

/* >>>> ** MEMORY MANAGEMENT ** <<<<
 * ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨
 * Stack : Think of a Stack of Pancakes. The Last in the Stack is the Last you will get
 * LIFO : Last in : First Out -- FIFO (Queue): Orders in the Way you take out 
 * CPU has Special Memory called Registers. Registers are Generic variables : Its Super fast
 *
 * Value types -- Native types (int double char) lowercase
 * int x = 0; < This is a Local Variable. OR Local Value Type.
 * Local Value Types are put in the STACK. Structured, no fragmentation.
 * Program using more STACK than HEAP.
 * 
 * >> Stack : You don't need to CALL the Garbage Collectors. Its Automatic.
 * >> Value types are NOT managed, because they live in the Stack :: and not the Heap ( not always true ) 
 * >> Reference types are managed, because they live in the Heap :: and not the stack ( not always true )
 * 
 * >>> A Reference type: Is a Pointer
 * >>> A Class : Is a REFERENCE type.
 * >>> Local variable lives in a stack, but if it belongs to a class, it points to the Heap : bad
 *
 * <<<< STRUCT >>>>
 * STRUCT is considered a VALUE TYPE :: Not a Reference Type
 * public struct Player {}
 * Player player = new Player()
 * This : is in the STACK, and not the HEAP
 * So Struct better for Memory.
 * But : Struct should almost never be used. No more than 16 bytes in size. Why?
 * - A Struct can have 4 Integers, * No more than that * 
 * - Because : STACK is Limited. Big STACK is bad.
 * 
 * <<<< MEMORY >>>>
 * ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨
 * MEMORY IS MADE OF : 
 * [ Stack ] : [ HEAP ]
 *
 * Memory POOL : is a way of taking care of the HEAP
 * "Garbage collector go away, I am in charge, don't touch this"
 * 
 * Avoid Heap Fragmentation. Pre-instantiate which takes data <>
 * >> Loading Times - Prefill Memory Pool
 * >> In a HackSlash, Fill Memory *Before* gameplay.
 * >> Lazy instantiation: If you have 100 enemies, *pretend* to instantiate them by MARKING them,
 * >>> Then Instantiate them when the player *sees* them 
 * 
 * Delete Class, GARBAGE COLLECTOR must Remove class from MEMORY, u have a (empty space) in HEAP
 * You make a = new AGAIN : Empty space is not used. HOLES take Memory, UNTIL Garbage Collection reclaims it.
 * 
 * TLDR: Instantiate and Delete creates Holes in HEAP,
 * This causes Memory FRAGMENTATION. Data-Driven-Development: Hates HEAP
 *
 * If you have "Enemies" in Memory in Different positions, and you ITERATE over the enemies,
 * They Enemies are NOT "together" like they are in an Array. Any instantiation has a random HEAP position.
 *
 * CACHING requires MEMORY to stay the same. CACHE MISS (if u can't allocate HEAP to Cache : bad)
 *
 * 
 * <<<< HEAP >>>>
 * ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨
 * Heap : Normal memory, RAM
 * Programming: Every time you use = new, you allocate More Memory to the HEAP
 *
 * 
 * <<<< STACK >>>>
 * ¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨¨
 * A STACK is LIMITED. You need a HEAP. If you make a Struct, it can only be 16 bytes max.
 * Memory Stack : Special Stack in your CPU. You have a Lot of it.
 * Functional programming requires Memory Stack. Old days - Slow : Today - Fast
 * 
 * Chunk of Memory that works like a Stack. Its Way Closer to the CPU, and more Structured
 * If Method [A] calls [B], they exist in memory and jumps from One Memory Address -> To Another Memory Address
 * After Method is called, it goes TO IT and BACK
 * 
 * Whenever you call a Method, it will STORE your CURRENT STATE in a STACK
 * 
 * TOP OF STACK -- 
 * [ 2 ] : [ 1 ]
 *
 * If you remove, they reorder
 * 
 * [ 1 ] : [ 2 ]
 * BOTTOM OF STACK --
 *
 * HIGH COHESION and LOW COUPLING is the GOAL.
 * More Coupling >
 * uInt > Make sure you don't have Negative numbers.
 * uInt > Points and makes sure you don't Store a negative number
 */
 
 
 /* Uses LIST to make a UI
  * List because it requires Positions for Reference
  * A List is a Stack
  * When the List is very long, the UI causes Stutter problems.
  * So : What is the best way to create a UI ? 
  */