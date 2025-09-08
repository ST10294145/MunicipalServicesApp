using System;


namespace MunicipalServicesApp
{
    public class IssueNode
    {
        public Issue Data { get; set; }
        public IssueNode Next { get; set; }

        public IssueNode(Issue issue)
        {
            Data = issue;
            Next = null;
        }
    }

    public class IssueLinkedList
    {
        private IssueNode head;
        private int nextId = 1;  // ✅ Track Issue IDs

        public void AddIssue(Issue issue)
        {
            issue.IssueID = GetNextId(); // ✅ assign ID automatically
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

        public int GetNextId()
        {
            return nextId++;
        }

        public List<Issue> GetAllIssues()
        {
            List<Issue> issues = new List<Issue>();
            IssueNode current = head;

            while (current != null)
            {
                issues.Add(current.Data);
                current = current.Next;
            }

            return issues;
        }
    }
}
