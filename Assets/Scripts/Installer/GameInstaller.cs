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

        Container.Bind<PoolManager>().AsSingle().NonLazy();

        
        Container.BindFactory<Object, GameSettings.ContainerObjectDetails, Transform, Button, Button.Factory>()
            .FromFactory<ButtonFactory>();

        Container.BindFactory<Object, ViewObject, ViewObject.Factory>()
            .FromFactory<ViewObjectFactory>();

        Container.DeclareSignal<ChangeTypeViewObjectSignal>();
        Container.DeclareSignal<ViewObjectInCenterSignal>();
        Container.DeclareSignal<CreateViewObjectSignal>();
        Container.DeclareSignal<DespawnedViewObjectSignal>();

        Container.BindInterfacesTo<Spawner>().AsSingle();
        
        Container.BindFactory<Presenter, Presenter.Factory>().AsSingle();
    }
}