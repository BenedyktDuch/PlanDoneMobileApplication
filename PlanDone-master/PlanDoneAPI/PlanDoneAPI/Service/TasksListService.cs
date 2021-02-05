using PlanDoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanDoneAPI.Service
{

    public class TasksListService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void AddTask(TasksList newTask)
        {
            newTask.TaskID = Guid.NewGuid().ToString();
            newTask.Done = false;
            newTask.CreateDate = DateTime.Now;
            newTask.Priority = 3;
            db.TasksLists.Add(newTask);
            db.SaveChanges();
        }
        public void DeleteTask(string taskID)
        {
            var taskToDelete = db.TasksLists.Where(x => x.TaskID == taskID).FirstOrDefault();
            db.TasksLists.Remove(taskToDelete);
            db.SaveChanges();
        }

        public void TaskDoneChange(string taskID)
        {
            var vTask = db.TasksLists.Where(x => x.TaskID == taskID).FirstOrDefault();
            if (vTask.Done)
                vTask.Done = false;
            else
                vTask.Done = true;

            db.SaveChanges();
        }
    }
}