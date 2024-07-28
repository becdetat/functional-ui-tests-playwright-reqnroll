"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

export default function Home() {
  const router = useRouter();
  const [ username, setUsername ] = useState("");
  const [ password, setPassword ] = useState("");
  const [ errorMessage, setErrorMessage ] = useState("")

  function signIn() {
    setErrorMessage("");

    if (username !== "rebecca" || password !== "p4$$w0rd") {
      setErrorMessage("Invalid username or password");
      return;
    }

    router.push("/secure");
  }

  return (
    <div>
      <p>Welcome to the Playwright and Reqnroll example application.</p>
      {errorMessage && (
        <p style={{ color: 'red' }}>{errorMessage}</p>
      )}
      <p><label>Username: <input type="text" onChange={(e) => setUsername(e.target.value)} /></label></p>
      <p><label>Password: <input type="password" onChange={(e) => setPassword(e.target.value)} /></label></p>
      <p><button onClick={signIn}>Sign in</button></p>
    </div>
  );
}

