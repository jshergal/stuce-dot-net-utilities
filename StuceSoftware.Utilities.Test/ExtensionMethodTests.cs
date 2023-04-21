namespace StuceSoftware.Utilities.Test;

public class ExtensionMethodTests
{
    private static readonly int[] SimpleArray = {1, 2, 3, 4};

    [Fact]
    public void GivenArray_WhenSliceOfWholeArray_ResultIsCopyOfArray()
    {
        var slice = SimpleArray.Slice(0, SimpleArray.Length);

        Assert.NotSame(SimpleArray, slice);
        Assert.Equal(SimpleArray, slice);
    }

    [Fact]
    public void GivenArray_WhenSlice_GivesNewArrayOfExpectedSize()
    {
        var slice = SimpleArray.Slice(0, 2);

        Assert.Equal(2, slice.Length);
        Assert.Equal(SimpleArray[0], slice[0]);
        Assert.Equal(SimpleArray[1], slice[1]);
    }

    [Fact]
    public void GivenArray_WhenSliceLastTwo_GivesNewArrayEndItems()
    {
        var slice = SimpleArray.Slice(SimpleArray.Length - 2, SimpleArray.Length);

        Assert.Equal(2, slice.Length);
        Assert.Equal(SimpleArray[^2], slice[0]);
        Assert.Equal(SimpleArray[^1], slice[1]);
    }

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(0, -1)]
    [InlineData(0, 10)]
    public void GivenArray_WhenSliceWithInvalidRange_ThrowsOutOfRangeException(int start, int end)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SimpleArray.Slice(start, end));
    }

    [Fact]
    public void GivenNullableType_WhenIsNullable_ReturnsTrue()
    {
        int? num = 0;
        Assert.True(num.IsNullable());
    }

    [Fact]
    public void GivenNonNullableType_WhenIsNullable_ReturnsFalse()
    {
        var num = 0;
        Assert.False(num.IsNullable());
    }

    [Fact]
    public void GivenNull_WhenIsNullable_ReturnsTrue()
    {
        object t = null!;
        Assert.True(t.IsNullable());
    }
}