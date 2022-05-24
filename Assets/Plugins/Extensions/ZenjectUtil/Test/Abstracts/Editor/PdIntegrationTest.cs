using NUnit.Framework;
using UnityEngine;
using Zenject;
using ZenjectUtil.Test.Extensions;

namespace Tests.Abstracts
{
	public abstract class PdIntegrationTest
	{

		[SetUp]
		public virtual void SetUp()
		{
			ResourceSubstitute.Clear();
			SubstituteResources();
		}

		protected abstract void SubstituteResources();

		[TearDown]
		public virtual void TearDown()
		{
			ResourceSubstitute.Clear();
			Object.Destroy(ProjectContext.Instance.gameObject);
		}
		
	}
}