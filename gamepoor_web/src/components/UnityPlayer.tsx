import {useEffect} from 'react'
import {useLocation} from 'react-router-dom'
import {Unity, useUnityContext} from 'react-unity-webgl'

declare global {
  interface Window {
    unityInstance: any
  }
}

export function UnityPlayer() {
  const {state} = useLocation()
  const {
    unityProvider,
    sendMessage,
    addEventListener,
    removeEventListener,
    isLoaded,
    UNSAFE__unityInstance,
    loadingProgression,
    requestFullscreen
  } = useUnityContext({
    loaderUrl: 'webgl/MSK/Build/MSK.loader.js',
    dataUrl: 'webgl/MSK/Build/MSK.data',
    frameworkUrl: 'webgl/MSK/Build/MSK.framework.js',
    codeUrl: 'webgl/MSK/Build/MSK.wasm',
    streamingAssetsUrl: 'StreamingAssets',
    companyName: 'DefaultCompany',
    productName: 'Monster Killer_T1',
    productVersion: '0.2'
  })

  const handleloadGameData = () => {
    if (isLoaded && window.unityInstance) {
      sendMessage('GameManager', 'LoadGameData')
    } else {
      console.error('Unity instance is not available yet.')
    }
  }

  useEffect(() => {
    const unityIcon = document.createElement('link')
    unityIcon.rel = 'shortcut icon'
    unityIcon.href = '/webgl/MSK/TemplateData/favicon.ico'
    document.head.appendChild(unityIcon)

    const unityCss = document.createElement('link')
    unityCss.rel = 'stylesheet'
    unityCss.href = '/webgl/MSK/TemplateData/style.css'
    document.head.appendChild(unityCss)

    if (isLoaded && UNSAFE__unityInstance !== null) {
      console.log('window.unityInstance' + window.unityInstance)
      window.unityInstance = UNSAFE__unityInstance
      if (state.playerID) {
        sendMessage('GameManager', 'LoadGameData', state.playerID)
      }
    }

    /*     addEventListener('LoadGameData', handleloadGameData)
    return () => {
      removeEventListener('LoadGameData', handleloadGameData)
    } */
  }, [
    isLoaded,
    UNSAFE__unityInstance,
    addEventListener,
    removeEventListener,
    handleloadGameData
  ])

  return (
    <div id="unity-container" className="unity-desktop">
      <div className="unity-loading-bar" style={{display: isLoaded ? 'none' : 'block'}}>
        <div id="unity-logo"></div>
        <div id="unity-progress-bar-empty">
          <div
            id="unity-progress-bar-full"
            style={{width: `${loadingProgression * 100}%`}}></div>
        </div>
      </div>
      <div id="unity-warning"></div>
      <Unity
        unityProvider={unityProvider}
        style={{width: '960px', height: '600px', border: '2px solid black'}}
      />
      <div id="unity-footer">
        <div id="unity-webgl-logo"></div>
        <button
          id="unity-fullscreen-button"
          onClick={() => requestFullscreen(true)}></button>
        <div id="unity-build-title">Monster Killer_T1</div>
        <button onClick={handleloadGameData}>loadGameData</button>
      </div>
    </div>
  )
}
