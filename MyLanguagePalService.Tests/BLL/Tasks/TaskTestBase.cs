using System.Collections.Generic;
using System.Data.Entity;
using Moq;
using MyLanguagePal.Core.Framework;
using MyLanguagePalService.DAL.Models;
using MyLanguagePalService.Tests.TestsShared;

namespace MyLanguagePalService.Tests.BLL.Tasks
{
    public abstract class TaskTestBase : TestBase
    {
        public Mock<IDbSet<TaskSettingsDal>> CreateTaskSettingsMockDbSet(IList<TaskSettingsDal> data)
        {
            return CreateMockDbSet(data, db => db.TaskSettings);
        }
    }
}