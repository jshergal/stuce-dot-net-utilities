namespace StuceSoftware.Utilities.Test;

public class FixedSizeStackTest
{
    [Fact]
    public void TestConstructorMaxSizeCannotBeZero()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeStack<int>(0));
    }

    [Fact]
    public void TestConstructorMaxSizeCannotBeLessThanZero()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeStack<int>(-1));
    }

    [Fact]
    public void TestConstructorKeepsFirstElementsFromIEnumerable()
    {
        var stack = new FixedSizeStack<int>(5, Enumerable.Range(0, 10));
        Assert.True(stack.SequenceEqual(Enumerable.Range(0, 5)));
    }

    [Fact]
    public void TestMaxSizeCannotSetZero()
    {
        var stack = new FixedSizeStack<int>(10);
        Assert.Throws<ArgumentOutOfRangeException>(() => stack.MaxSize = 0);
    }

    [Fact]
    public void TestMaxSizeCannotSetLessThanZero()
    {
        var stack = new FixedSizeStack<int>(10);
        Assert.Throws<ArgumentOutOfRangeException>(() => stack.MaxSize = -1);
    }

    [Fact]
    public void TestCannotPeekEmptyStack()
    {
        var stack = new FixedSizeStack<int>(10);
        Assert.Throws<InvalidOperationException>(() => stack.Peek());
    }

    [Fact]
    public void TestCannotPopEmptyStack()
    {
        var stack = new FixedSizeStack<int>(10);
        Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }

    [Fact]
    public void TestPushSuccessful()
    {
        const int valueToPush = 42;
        var stack = new FixedSizeStack<int>(10);
        Assert.Empty(stack);

        stack.Push(valueToPush);

        Assert.Single(stack);
        Assert.Equal(valueToPush, stack.Peek());
    }

    [Fact]
    public void TestPeekSuccessful()
    {
        const int valueToPush = 42;
        var stack = new FixedSizeStack<int>(10);
        Assert.Empty(stack);

        stack.Push(valueToPush);
        Assert.Single(stack);

        Assert.Equal(valueToPush, stack.Peek());
        Assert.Single(stack);
    }

    [Fact]
    public void TestPopSuccessful()
    {
        const int valueToPush = 42;
        var stack = new FixedSizeStack<int>(10);
        Assert.Empty(stack);

        stack.Push(valueToPush);
        Assert.Single(stack);

        Assert.Equal(valueToPush, stack.Pop());
        Assert.Empty(stack);
    }

    [Fact]
    public void TestPushBeyondMaxSizeWillNotIncreaseSize()
    {
        var stack = new FixedSizeStack<int>(10);
        for (var i = 0; i < 100; ++i) stack.Push(i);
        Assert.Equal(10, stack.Count);
    }

    [Fact]
    public void TestPushBeyondMaxSizeDumpsOlderElements()
    {
        var stack = new FixedSizeStack<int>(10);
        for (var i = 0; i < 11; ++i) stack.Push(i);
        Assert.DoesNotContain(0, stack);
        Assert.Contains(10, stack);
    }

    [Fact]
    public void TestSetMaxSizeSmallerDumpsElements()
    {
        var stack = new FixedSizeStack<int>(10);
        for (var i = 0; i < 10; ++i) stack.Push(i);
        Assert.Equal(10, stack.Count);

        stack.MaxSize = 5;
        Assert.Equal(5, stack.Count);
        for (var i = 0; i < 5; ++i) Assert.DoesNotContain(i, stack);
    }

    [Fact]
    public void TestSetMaxSizeWillNotDumpWhenLessElements()
    {
        var stack = new FixedSizeStack<int>(10, Enumerable.Range(0, 5));
        Assert.Equal(5, stack.Count);

        stack.MaxSize = 6;
        Assert.Equal(5, stack.Count);
    }

    [Fact]
    public void TestToArray()
    {
        var array = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        var stack = new FixedSizeStack<int>(array.Length);
        for (var i = 1; i <= array.Length; ++i) stack.Push(array[^i]);

        var test = stack.ToArray();
        Assert.Equal(array, test);
    }

    [Fact]
    public void TestClearEmptiesStack()
    {
        var stack = new FixedSizeStack<int>(50, Enumerable.Range(0, 50));
        Assert.Equal(50, stack.Count);

        stack.Clear();
        Assert.Empty(stack);
    }
}