import './App.css'
import { ReactNode, useState } from 'react';
import Navigation from './Components/Navigation'
import Home from './Views/Home'
import Login from './Views/Login'
import { Route, Routes} from 'react-router-dom';
import {theme, Layout } from 'antd';

type route = {
  path: string,
  element: ReactNode,
}


function App() {
  const [collapsed, setCollapsed] = useState(false);
  const { Header, Footer, Content, Sider } = Layout;
  const routes: Array<route> = [
   {path: '/', element: <Home /> },
   {path: '/login', element: <Login /> },
 ]
    

  
 const {token: { colorBgContainer },} = theme.useToken();
    return (
      <Layout style={{ minHeight: '100vh' }}>
        <Sider collapsible collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
          <Navigation/>
        </Sider>
        <Layout >
          <Header style={{ padding: 0, background: colorBgContainer }}>
                  Content
          </Header>
          <Content style={{ margin: '0 16px' }}>
            <div  style={{ margin: '16px 0' }}>
              <Routes >
                {routes.map((route, index) =>  (<Route key={index} path={route.path} element={route.element}/> ))}
              </Routes> 
            </div>
          </Content>
          <Footer style={{ textAlign: 'center' }}>©DerPizzaBursche 2023</Footer>
        </Layout>           
      </Layout>
  )
}

export default App
