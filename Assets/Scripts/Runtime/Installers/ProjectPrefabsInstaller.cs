using Runtime.Data.Base.Audio;
using Runtime.Data.Base.Audio.Impl;
using Runtime.Data.Base.FX;
using Runtime.Data.Base.FX.Impl;
using Runtime.Data.Base.Materials.Impls;
using Runtime.Data.Base.Prefabs;
using Runtime.Data.Base.Prefabs.Impl;
using Runtime.Data.Base.Ui;
using Runtime.Data.Base.Ui.Impl;
using Runtime.Data.PlayerData.Currency;
using Runtime.Data.PlayerData.Currency.Impls;
using Runtime.Data.PlayerData.LayoutSpriteData;
using Runtime.Data.PlayerData.LayoutSpriteData.Impls;
using Runtime.Data.PlayerData.Levels;
using Runtime.Data.PlayerData.Levels.Impls;
using Runtime.Data.PlayerData.Recipe;
using Runtime.Data.PlayerData.Recipe.Impls;
using Runtime.Data.PlayerData.Skins;
using Runtime.Data.PlayerData.Skins.Impls;
using UnityEngine;
using Zenject;
using ZenjectUtil.Test.Extensions;

namespace Runtime.Installers
{
    [CreateAssetMenu(menuName = "Installers/ProjectPrefabsInstaller", fileName = "ProjectPrefabsInstaller")]
    public class ProjectPrefabsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private PrefabsBase prefabsBase;
        [SerializeField] private UiPrefabsBase uiPrefabsBase;
        [SerializeField] private MaterialsBase materialsBase;
        [SerializeField] private AudioBase audioBase;
        [SerializeField] private FxBase fxBase;
        [SerializeField] private SkinsData skinsData;
        [SerializeField] private LevelsData levelsData;
        [SerializeField] private CurrenciesData currenciesData;
        [SerializeField] private LayoutSpiteData _layoutSpiteData; 
        [SerializeField] private RecipeData recipeData; 
        
        public override void InstallBindings()
        {
            Container.Bind<IPrefabsBase>().FromSubstitute(prefabsBase).AsSingle();
            Container.Bind<IUiPrefabsBase>().FromSubstitute(uiPrefabsBase).AsSingle();
            Container.Bind<MaterialsBase>().FromSubstitute(materialsBase).AsSingle();
            Container.Bind<IAudioBase>().FromSubstitute(audioBase).AsSingle();
            Container.Bind<IFxBase>().FromSubstitute(fxBase).AsSingle();
            Container.Bind<ISkinsData>().FromSubstitute(skinsData).AsSingle();
            Container.Bind<ILevelsData>().FromSubstitute(levelsData).AsSingle();
            Container.Bind<ICurrenciesData>().FromSubstitute(currenciesData).AsSingle();
            Container.Bind<IRecipeData>().FromSubstitute(recipeData).AsSingle();
            Container.Bind<ILayoutSpriteData>().FromSubstitute(_layoutSpiteData).AsSingle();
        }
    }
}