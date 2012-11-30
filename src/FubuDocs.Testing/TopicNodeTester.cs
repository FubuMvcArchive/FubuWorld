using System;
using NUnit.Framework;
using FubuTestingSupport;
using System.Linq;

namespace FubuDocs.Testing
{
    [TestFixture]
    public class TopicNodeTester
    {
        private TopicNode a;
        private TopicNode b;
        private TopicNode c;
        private TopicNode d;
        private TopicNode e;

        [SetUp]
        public void SetUp()
        {
            a = TopicNode.For<ATopic>();
            b = TopicNode.For<BTopic>();
            c = TopicNode.For<CTopic>();
            d = TopicNode.For<DTopic>();
            e = TopicNode.For<ETopic>();
        }

        [Test]
        public void is_topic_type_positive()
        {
            TopicNode.IsTopicType(typeof(ATopic)).ShouldBeTrue();
        }

        [Test]
        public void is_topic_negative()
        {
            TopicNode.IsTopicType(GetType()).ShouldBeFalse();
        }

        [Test]
        public void blow_up_if_you_try_to_create_a_node_for_the_wrong_type()
        {
            Exception<ArgumentOutOfRangeException>.ShouldBeThrownBy(() => {
                new TopicNode(GetType());
            });
        }

        [Test]
        public void get_the_title_in_a_node()
        {
            new TopicNode(typeof (ATopic))
                .Title
                .ShouldEqual("A topic");
        }

        [Test]
        public void initial_state_nulls()
        {
            a.NextSibling.ShouldBeNull();
            a.Parent.ShouldBeNull();
            a.PreviousSibling.ShouldBeNull();
            a.LastChild.ShouldBeNull();

            a.ChildNodes.Any().ShouldBeFalse();
        }

        [Test]
        public void append_one_child()
        {
            a.AppendChild(b);

            a.FirstChild.ShouldBeTheSameAs(b);
            a.LastChild.ShouldBeTheSameAs(b);
            a.ChildNodes.ShouldHaveTheSameElementsAs(b);

            b.Parent.ShouldBeTheSameAs(a);
            b.NextSibling.ShouldBeNull();
            b.PreviousSibling.ShouldBeNull();

        }

        [Test]
        public void prepend_one_child()
        {
            a.PrependChild(b);

            a.FirstChild.ShouldBeTheSameAs(b);
            a.LastChild.ShouldBeTheSameAs(b);
            a.ChildNodes.ShouldHaveTheSameElementsAs(b);

            b.Parent.ShouldBeTheSameAs(a);
            b.NextSibling.ShouldBeNull();
            b.PreviousSibling.ShouldBeNull();
        }

        [Test]
        public void append_multiple_children()
        {
            a.AppendChild(b);
            a.AppendChild(c);
            a.AppendChild(d);

            A_should_Have_B_then_C_then_D();
        }

        private void A_should_Have_B_then_C_then_D()
        {

            a.FirstChild.ShouldBeTheSameAs(b);
            a.LastChild.ShouldBeTheSameAs(d);
            a.ChildNodes.ShouldHaveTheSameElementsAs(b, c, d);

            b.PreviousSibling.ShouldBeNull();
            b.NextSibling.ShouldBeTheSameAs(c);
            b.Parent.ShouldBeTheSameAs(a);

            c.PreviousSibling.ShouldBeTheSameAs(b);
            c.NextSibling.ShouldBeTheSameAs(d);
            b.Parent.ShouldBeTheSameAs(a);

            d.PreviousSibling.ShouldBeTheSameAs(c);
            d.NextSibling.ShouldBeNull();
            b.Parent.ShouldBeTheSameAs(a);
        }

        [Test]
        public void prepend_child()
        {
            a.AppendChild(c);
            a.AppendChild(d);

            a.PrependChild(b);

            A_should_Have_B_then_C_then_D();
        }

        [Test]
        public void insert_before_as_the_first_child()
        {
            a.AppendChild(c);
            a.AppendChild(d);

            c.InsertBefore(b);

            A_should_Have_B_then_C_then_D();
        }

        [Test]
        public void insert_before_in_the_middle_with_a_parent()
        {
            a.AppendChild(b);
            a.AppendChild(d);

            d.InsertBefore(c);

            A_should_Have_B_then_C_then_D();
        }

        [Test]
        public void insert_before_with_no_parent()
        {
            b.InsertBefore(c);

            b.PreviousSibling.ShouldBeTheSameAs(c);
            c.NextSibling.ShouldBeTheSameAs(b);

            b.NextSibling.ShouldBeNull();
            c.PreviousSibling.ShouldBeNull();

            b.Parent.ShouldBeNull();
            c.Parent.ShouldBeNull();
        }

        [Test]
        public void remove_from_end_of_stack()
        {
            a.AppendChild(b);
            a.AppendChild(c);
            a.AppendChild(d);
            a.AppendChild(e);

            e.Remove();

            A_should_Have_B_then_C_then_D();
        }

        [Test]
        public void remove_from_middle()
        {
            a.AppendChild(b);
            a.AppendChild(e);
            a.AppendChild(c);
            a.AppendChild(d);
            

            e.Remove();

            A_should_Have_B_then_C_then_D();
        }

        [Test]
        public void remove_from_top()
        {
            a.AppendChild(e);
            a.AppendChild(b);
            a.AppendChild(c);
            a.AppendChild(d);


            e.Remove();

            A_should_Have_B_then_C_then_D();
        }

        [Test]
        public void remove_only_child()
        {
            a.AppendChild(e);
            e.Remove();

            initial_state_nulls();
        }
    }

    public class BTopic : Topic
    {
        public BTopic()
            : base("B topic")
        {
        }
    }

    public class CTopic : Topic
    {
        public CTopic()
            : base("C topic")
        {
        }
    }

    public class DTopic : Topic
    {
        public DTopic()
            : base("D topic")
        {
        }
    }

    public class ETopic : Topic
    {
        public ETopic()
            : base("E topic")
        {
        }
    }
}