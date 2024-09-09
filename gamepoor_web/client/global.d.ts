// client/global.d.ts
declare function createUnityInstance(
  canvas: HTMLCanvasElement | null,
  config: {
    dataUrl: string;
    frameworkUrl: string;
    codeUrl: string;
    streamingAssetsUrl?: string;
    companyName?: string;
    productName?: string;
    productVersion?: string;
    showBanner?: (msg: string, type: string) => void;
  },
  onProgress?: (progress: number) => void
): Promise<any>;
