﻿using System;
using System.IO;
using System.Linq;
using FlatFiles.TypeMapping;
using Xunit;

namespace FlatFiles.Test
{
    public class FixedLengthWriterMetadataTester
    {
        [Fact]
        public void TestWriter_WithSchema_SchemaNotCounted()
        {
            var outputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            outputMapper.Property(x => x.Name, 10);
            outputMapper.CustomProperty(x => x.RecordNumber, new RecordNumberColumn("RecordNumber"), 10);
            outputMapper.Property(x => x.CreatedOn, 10).OutputFormat("MM/dd/yyyy");

            var people = new[]
            {
                new Person() { Name = "Bob", CreatedOn = new DateTime(2018, 04, 25) },
                new Person() { Name = "Tom", CreatedOn = new DateTime(2018, 04, 26) },
                new Person() { Name = "Jane", CreatedOn = new DateTime(2018, 04, 27) }
            };

            StringWriter writer = new StringWriter();
            outputMapper.Write(writer, people, new FixedLengthOptions() { IsFirstRecordHeader = true });
            string output = writer.ToString();

            var inputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            inputMapper.Property(x => x.Name, 10);
            inputMapper.Property(x => x.RecordNumber, 10);
            inputMapper.Property(x => x.CreatedOn, 10).InputFormat("MM/dd/yyyy");

            StringReader reader = new StringReader(output);
            var results = inputMapper.Read(reader, new FixedLengthOptions() { IsFirstRecordHeader = true }).ToArray();
            Assert.Equal(3, results.Length);
            Assert.Equal("Bob", results[0].Name);
            Assert.Equal(1, results[0].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 25), results[0].CreatedOn);
            Assert.Equal("Tom", results[1].Name);
            Assert.Equal(2, results[1].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 26), results[1].CreatedOn);
            Assert.Equal("Jane", results[2].Name);
            Assert.Equal(3, results[2].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 27), results[2].CreatedOn);
        }

        [Fact]
        public void TestWriter_WithSchema_SchemaCounted()
        {
            var outputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            outputMapper.Property(x => x.Name, 10);
            outputMapper.CustomProperty(x => x.RecordNumber, new RecordNumberColumn("RecordNumber")
            {
                IncludeSchema = true
            }, 10);
            outputMapper.Property(x => x.CreatedOn, 10).OutputFormat("MM/dd/yyyy");

            var people = new[]
            {
                new Person() { Name = "Bob", CreatedOn = new DateTime(2018, 04, 25) },
                new Person() { Name = "Tom", CreatedOn = new DateTime(2018, 04, 26) },
                new Person() { Name = "Jane", CreatedOn = new DateTime(2018, 04, 27) }
            };

            StringWriter writer = new StringWriter();
            outputMapper.Write(writer, people, new FixedLengthOptions() { IsFirstRecordHeader = true });
            string output = writer.ToString();

            var inputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            inputMapper.Property(x => x.Name, 10);
            inputMapper.Property(x => x.RecordNumber, 10);
            inputMapper.Property(x => x.CreatedOn, 10).InputFormat("MM/dd/yyyy");

            StringReader reader = new StringReader(output);
            var results = inputMapper.Read(reader, new FixedLengthOptions() { IsFirstRecordHeader = true }).ToArray();
            Assert.Equal(3, results.Length);
            Assert.Equal("Bob", results[0].Name);
            Assert.Equal(2, results[0].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 25), results[0].CreatedOn);
            Assert.Equal("Tom", results[1].Name);
            Assert.Equal(3, results[1].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 26), results[1].CreatedOn);
            Assert.Equal("Jane", results[2].Name);
            Assert.Equal(4, results[2].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 27), results[2].CreatedOn);
        }

        [Fact]
        public void TestWriter_NoSchema_SchemaNotCounted()
        {
            var outputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            outputMapper.Property(x => x.Name, 10);
            outputMapper.CustomProperty(x => x.RecordNumber, new RecordNumberColumn("RecordNumber")
            {
                IncludeSchema = false
            }, 10);
            outputMapper.Property(x => x.CreatedOn, 10).OutputFormat("MM/dd/yyyy");

            var people = new[]
            {
                new Person() { Name = "Bob", CreatedOn = new DateTime(2018, 04, 25) },
                new Person() { Name = "Tom", CreatedOn = new DateTime(2018, 04, 26) },
                new Person() { Name = "Jane", CreatedOn = new DateTime(2018, 04, 27) }
            };

            StringWriter writer = new StringWriter();
            outputMapper.Write(writer, people, new FixedLengthOptions() { IsFirstRecordHeader = true });
            string output = writer.ToString();

            var inputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            inputMapper.Property(x => x.Name, 10);
            inputMapper.Property(x => x.RecordNumber, 10);
            inputMapper.Property(x => x.CreatedOn, 10).InputFormat("MM/dd/yyyy");

            StringReader reader = new StringReader(output);
            var results = inputMapper.Read(reader, new FixedLengthOptions() { IsFirstRecordHeader = true }).ToArray();
            Assert.Equal(3, results.Length);
            Assert.Equal("Bob", results[0].Name);
            Assert.Equal(1, results[0].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 25), results[0].CreatedOn);
            Assert.Equal("Tom", results[1].Name);
            Assert.Equal(2, results[1].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 26), results[1].CreatedOn);
            Assert.Equal("Jane", results[2].Name);
            Assert.Equal(3, results[2].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 27), results[2].CreatedOn);
        }
        
        [Fact]
        public void TestWriter_WithSchema_WithIgnoredColumns()
        {
            var outputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            outputMapper.Property(x => x.Name, 10);
            outputMapper.Ignored(1);
            outputMapper.CustomProperty(x => x.RecordNumber, new RecordNumberColumn("RecordNumber")
            {
                IncludeSchema = true
            }, 10);
            outputMapper.Ignored(1);
            outputMapper.Property(x => x.CreatedOn, 10).OutputFormat("MM/dd/yyyy");

            var people = new[]
            {
                new Person() { Name = "Bob", CreatedOn = new DateTime(2018, 04, 25) },
                new Person() { Name = "Tom", CreatedOn = new DateTime(2018, 04, 26) },
                new Person() { Name = "Jane", CreatedOn = new DateTime(2018, 04, 27) }
            };

            StringWriter writer = new StringWriter();
            outputMapper.Write(writer, people, new FixedLengthOptions() { IsFirstRecordHeader = true });
            string output = writer.ToString();

            var inputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            inputMapper.Property(x => x.Name, 10);
            inputMapper.Ignored(1);
            inputMapper.Property(x => x.RecordNumber, 10);
            inputMapper.Ignored(1);
            inputMapper.Property(x => x.CreatedOn, 10).InputFormat("MM/dd/yyyy");

            StringReader reader = new StringReader(output);
            var results = inputMapper.Read(reader, new FixedLengthOptions() { IsFirstRecordHeader = true }).ToArray();
            Assert.Equal(3, results.Length);
            Assert.Equal("Bob", results[0].Name);
            Assert.Equal(2, results[0].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 25), results[0].CreatedOn);
            Assert.Equal("Tom", results[1].Name);
            Assert.Equal(3, results[1].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 26), results[1].CreatedOn);
            Assert.Equal("Jane", results[2].Name);
            Assert.Equal(4, results[2].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 27), results[2].CreatedOn);
        }

        [Fact]
        public void TestWriter_NoRecordSeparator_ValidRecordCounts()
        {
            var outputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            outputMapper.Property(x => x.Name, 10);
            outputMapper.Ignored(1);
            outputMapper.CustomProperty(x => x.RecordNumber, new RecordNumberColumn("RecordNumber")
            {
                IncludeSchema = true
            }, 10);
            outputMapper.Ignored(1);
            outputMapper.Property(x => x.CreatedOn, 10).OutputFormat("MM/dd/yyyy");

            var people = new[]
            {
                new Person() { Name = "Bob", CreatedOn = new DateTime(2018, 04, 25) },
                new Person() { Name = "Tom", CreatedOn = new DateTime(2018, 04, 26) },
                new Person() { Name = "Jane", CreatedOn = new DateTime(2018, 04, 27) }
            };

            StringWriter writer = new StringWriter();
            outputMapper.Write(writer, people, new FixedLengthOptions()
            {
                IsFirstRecordHeader = true,
                HasRecordSeparator = false
            });
            string output = writer.ToString();

            var inputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            inputMapper.Property(x => x.Name, 10);
            inputMapper.Ignored(1);
            inputMapper.Property(x => x.RecordNumber, 10);
            inputMapper.Ignored(1);
            inputMapper.Property(x => x.CreatedOn, 10).InputFormat("MM/dd/yyyy");

            StringReader reader = new StringReader(output);
            var results = inputMapper.Read(reader, new FixedLengthOptions()
            {
                IsFirstRecordHeader = true,
                HasRecordSeparator = false
            }).ToArray();
            Assert.Equal(3, results.Length);
            Assert.Equal("Bob", results[0].Name);
            Assert.Equal(2, results[0].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 25), results[0].CreatedOn);
            Assert.Equal("Tom", results[1].Name);
            Assert.Equal(3, results[1].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 26), results[1].CreatedOn);
            Assert.Equal("Jane", results[2].Name);
            Assert.Equal(4, results[2].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 27), results[2].CreatedOn);
        }

        [Fact]
        public void TestWriter_WriteOnlyColumn_WithIgnoredColumn()
        {
            var outputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            outputMapper.Property(x => x.Name, 10);
            outputMapper.Ignored(1);
            outputMapper.WriteOnlyProperty("RecordNumber", new RecordNumberColumn("RecordNumber")
            {
                IncludeSchema = true
            }, 10);
            outputMapper.Ignored(1);
            outputMapper.Property(x => x.CreatedOn, 10).OutputFormat("MM/dd/yyyy");

            var people = new[]
            {
                new Person() { Name = "Bob", CreatedOn = new DateTime(2018, 04, 25) },
                new Person() { Name = "Tom", CreatedOn = new DateTime(2018, 04, 26) },
                new Person() { Name = "Jane", CreatedOn = new DateTime(2018, 04, 27) }
            };

            StringWriter writer = new StringWriter();
            outputMapper.Write(writer, people, new FixedLengthOptions() { IsFirstRecordHeader = true });
            string output = writer.ToString();

            var inputMapper = new FixedLengthTypeMapper<Person>(() => new Person());
            inputMapper.Property(x => x.Name, 10);
            inputMapper.Ignored(1);
            inputMapper.Property(x => x.RecordNumber, 10);
            inputMapper.Ignored(1);
            inputMapper.Property(x => x.CreatedOn, 10).InputFormat("MM/dd/yyyy");

            StringReader reader = new StringReader(output);
            var results = inputMapper.Read(reader, new FixedLengthOptions() { IsFirstRecordHeader = true }).ToArray();
            Assert.Equal(3, results.Length);
            Assert.Equal("Bob", results[0].Name);
            Assert.Equal(2, results[0].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 25), results[0].CreatedOn);
            Assert.Equal("Tom", results[1].Name);
            Assert.Equal(3, results[1].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 26), results[1].CreatedOn);
            Assert.Equal("Jane", results[2].Name);
            Assert.Equal(4, results[2].RecordNumber);
            Assert.Equal(new DateTime(2018, 04, 27), results[2].CreatedOn);
        }

        public class Person
        {
            public string Name { get; set; }

            public int RecordNumber { get; set; }

            public DateTime CreatedOn { get; set; }
        }
    }
}
