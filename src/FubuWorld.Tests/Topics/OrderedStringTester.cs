using System.Collections.Generic;
using FubuTestingSupport;
using FubuWorld.Topics;
using NUnit.Framework;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class OrderedStringTester
    {
        [Test]
        public void parses()
        {
            var s = new OrderedString("1.1.foo");
            s.Value.ShouldEqual("foo");
            s.Order.ShouldEqual(new int[] {1, 1});

            s = new OrderedString("1.2.3.foo");
            s.Value.ShouldEqual("foo");
            s.Order.ShouldEqual(new int[] { 1, 2, 3 });

            s = new OrderedString("foo");
            s.Value.ShouldEqual("foo");
            s.Order.ShouldEqual(new int[0]);
        }

        [Test]
        public void ordered_testing_is_alphabetic_if_no_ordered()
        {
            var s1 = new OrderedString("a");
            var s2 = new OrderedString("f");
            var s3 = new OrderedString("b");
            var s4 = new OrderedString("d");
            var s5 = new OrderedString("c");

            var list = new List<OrderedString>
            {
                s1,
                s2,
                s3,
                s4,
                s5
            };

            list.Sort();

            list.ShouldHaveTheSameElementsAs(s1, s3, s5, s4, s2);
        }

        [Test]
        public void ordered_testing_with_rank()
        {
            var s1 = new OrderedString("1.b");
            var s2 = new OrderedString("2.a");

            s2.CompareTo(s1).ShouldEqual(1);
            s1.CompareTo(s2).ShouldEqual(-1);
            s1.CompareTo(s1).ShouldEqual(0);
            s2.CompareTo(s2).ShouldEqual(0);
        }

        [Test]
        public void ordered_testing_with_higher_rank_rank()
        {
            var s1 = new OrderedString("1.1.1.b");
            var s2 = new OrderedString("1.1.2.a");

            s2.CompareTo(s1).ShouldEqual(1);
            s1.CompareTo(s2).ShouldEqual(-1);
            s1.CompareTo(s1).ShouldEqual(0);
            s2.CompareTo(s2).ShouldEqual(0);
        }
    }
}