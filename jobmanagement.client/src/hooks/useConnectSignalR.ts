import { useState } from "react";
import { startSignalRConnection } from "../services/jobManager/signalR/jobManagerSignalR";

export const useConnectSignalR = (): [
  () => Promise<void>,
  boolean,
  boolean | undefined
] => {
  const [isConnecting, setIsConnecting] = useState(false);
  const [result, setResult] = useState<any>();

  const connect = async () => {
    setIsConnecting(true);
    setResult(undefined);

    try {
      await startSignalRConnection();
      setResult(true)
    } catch (error) {
      console.error(error);
      setResult(false)
    } finally {
      setIsConnecting(false);
    }
  };

  return [connect, isConnecting, result];
};
