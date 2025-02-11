import { useState } from 'react'
import GetUser from './GetUser'
import CreateUser from './CreateUser'

function App() {
  return (
    <>
    <div style={{position: "absolute", left: "35%", top: "2%"}}>
      <h1>Welcome to testing</h1>
      <p>In here i will test different calls to our api "LagerstyringAPI"</p>
    </div>
    <div>
      <GetUser />
    </div>
    <div>
      <CreateUser />
    </div>
    </>
  )
}

export default App
