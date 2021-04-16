using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject] private GameSettings _settings;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        Container.BindFactory<GameSettings.ContainerObjectDetails, Transform, Button, Button.Factory>()
            .FromComponentInNewPrefab(_settings.ButtonPrefab)
            .WithGameObjectName("Button");

        Container.BindFactory<GameSettings.ContainerObjectDetails, ViewObject, ViewObject.Factory>()
            .FromPoolableMemoryPool<GameSettings.ContainerObjectDetails, ViewObject, ViewObjectPool>(
                x=>x.WithInitialSize(10)
                    .FromComponentInNewPrefab(_settings.ViewObjectPrefab)
                    .WithGameObjectName("View"));

        Container.DeclareSignal<ChangeTypeViewObjectSignal>();
        Container.DeclareSignal<ViewObjectInCenterSignal>();
        Container.DeclareSignal<CreateViewObjectSignal>();
        Container.DeclareSignal<DespawnedViewObjectSignal>();

        Container.BindInterfacesTo<Spawner>().AsSingle();
        
        Container.BindFactory<Presenter, Presenter.Factory>().AsSingle();


        //Container.BindFactory<Object, int, FooTest, FooTest.Factory>().FromFactory<FooTestFactory>();

        Container.BindFactory<Object, int, FooTest, FooTest.Factory>().FromPoolableMemoryPool<Object, int, FooTest, FooTestPool>(x=>x.WithInitialSize(10).FromFactory<FooTestFactory>());
        
    }

    public class ViewObjectPool : MonoPoolableMemoryPool<GameSettings.ContainerObjectDetails, IMemoryPool, ViewObject> { }

    //IPoolable<GameSettings.ContainerObjectDetails, IMemoryPool>
    public class FooTestPool : MonoPoolableMemoryPool<Object, int, IMemoryPool, FooTest> { }
}