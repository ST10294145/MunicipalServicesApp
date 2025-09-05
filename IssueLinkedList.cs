using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServicesApp
{
    // Node in the linked list
    public class IssueNode
    {
        public Issue Data { get; set; }
        public IssueNode Next { get; set; }

        public IssueNode(Issue data)
        {
            Data = data;
            Next = null;
        }
    }

    // Custom linked list to store issues
    public class IssueLinkedList
    {
        private IssueNode head;
        private int counter = 0;

        // Add a new issue to the list
        public void AddIssue(Issue issue)
        {
            counter++;
            issue.Id = counter; // auto-generate issue ID

            IssueNode newNode = new IssueNode(issue);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                IssueNode current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        // Retrieve all issues as IEnumerable
        public IEnumerable<Issue> GetAllIssues()
        {
            IssueNode current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}
