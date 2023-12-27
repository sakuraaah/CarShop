import React, { useState, useEffect, useContext } from 'react';
import { useLocation } from "react-router-dom";
import { Container } from 'reactstrap';
import FadeIn from 'react-fade-in';
import { faHouse } from '@fortawesome/free-solid-svg-icons'
import { NavMenu } from './NavMenu';
import { Layout as AntdLayout, Menu } from 'antd';
import {
  CarOutlined,
  DollarOutlined,
  FileTextOutlined,
  LoginOutlined,
  LogoutOutlined,
  SecurityScanOutlined,
  UserOutlined,
  UserAddOutlined,
} from '@ant-design/icons';
import { 
  SiderInfoWrapper,
  SiderLogoLink,
  SiderLogoWrapper
} from '../styles/layout/form';
import { 
  Avatar,
  Icon,
  LabelFormItem,
  Link
} from '../ui';
import authService from '../components/api-authorization/AuthorizeService'
import { ApplicationPaths } from '../components/api-authorization/ApiAuthorizationConstants';
import { UserDataContext } from '../contexts/UserDataProvider'

const { Content, Footer, Sider } = AntdLayout;

export const Layout = ({children}) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [collapsed, setCollapsed] = useState(false)

  const location = useLocation();
  const { data: userData } = useContext(UserDataContext)

  useEffect(() => {
    checkAuth()
  }, [])

  const checkAuth = async () => {
    const authenticated = await authService.isAuthenticated()
    setIsAuthenticated(authenticated)
  }

  const findActiveLink = (url, menuItems) => {
    for (const menuItem of menuItems) {
      if (url.includes('profile')) {
        return '4'
      } else if (url == menuItem?.label?.props?.href) {
        return String(menuItem.key)
      }
    }

    return null;
  }

  const mainMenuProfile = (role) => {
    switch (role) {
      case 'Seller':
        return {   
          key: 4,
          icon: <UserOutlined />,
          label: 'Profile',
          children: [
            {
              type: 'group',
              label: 'Profile',
              children: [
                {
                  key: 5,
                  label: <Link href="/profile">View Profile</Link>,
                }
              ],
            },
            {
              type: 'group',
              label: 'Your items',
              children: [
                {
                  key: 6,
                  label: <Link href="/profile/for-sale">Your vehicles</Link>,
                },
                {
                  key: 7,
                  label: <Link href="/profile/rental">Your Rentals</Link>,
                }
              ],
            },
          ]
        }

      case 'Buyer':
        return {   
          key: 4,
          icon: <UserOutlined />,
          label: 'Profile',
          children: [
            {
              type: 'group',
              label: 'Profile',
              children: [
                {
                  key: 5,
                  label: <Link href="/profile">View Profile</Link>,
                }
              ],
            },
            {
              type: 'group',
              label: 'Your items',
              children: [
                {
                  key: 6,
                  label: <Link href="/profile/bought-vehicles">Your bought vehicles</Link>,
                },
                {
                  key: 7,
                  label: <Link href="/profile/rent-orders">Your Rent Orders</Link>,
                }
              ],
            },
          ]
        }

      case 'Admin':
        return {
          key: 4,
          icon: <UserOutlined />,
          label: <Link href="/profile">Profile</Link>
        }

      default:
        return null
    }
  } 

  const mainMenuItems = [
    {
      key: 1,
      icon: <CarOutlined />,
      label: <Link href="/">Buy</Link>
    },
    {
      key: 2,
      icon: <DollarOutlined />,
      label: <Link href="/rental">Rental</Link>
    },
    ...[['Admin', 'Seller'].includes(userData?.role) &&
      {
        key: 3,
        icon: <FileTextOutlined />,
        label: <Link href="/rent-submissions">Submissions</Link>
      }
    ],
    ...(!isAuthenticated ? [
      {
        key: 4,
        icon: <UserAddOutlined />,
        label: <Link href={ApplicationPaths.Register}>Register</Link>
      },
      {
        key: 5,
        icon: <LoginOutlined />,
        label: <Link href={ApplicationPaths.Login}>Login</Link>
      }
    ] : [
      mainMenuProfile(userData?.role),
      ...[userData?.role == 'Admin' &&
        {
          key: 8,
          icon: <SecurityScanOutlined />,
          label: 'Admin Console',
          children: [
            {
              key: 9,
              label: <Link href="/admin/for-sale">Check all vehicles</Link>,
            },
            {
              key: 10,
              label: <Link href="/admin/rental">Check all rentals</Link>,
            }
          ],
        }
      ],
      {
        key: 11,
        icon: <LogoutOutlined />,
        label: <Link href={ApplicationPaths.LogOut}>Logout</Link>
      }
    ]),
  ]

  const activeLink = findActiveLink(location.pathname, mainMenuItems);

  const siderMenuItems = isAuthenticated ? [
    {
      key: 1,
      icon: <UserOutlined />,
      label: <Link href="/profile/edit" newPage>Edit Profile</Link>
    },
    {
      key: 2,
      icon: <LogoutOutlined />,
      label: <Link href={ApplicationPaths.LogOut}>Logout</Link>
    }
  ] : [
    {
      key: 1,
      icon: <UserAddOutlined />,
      label: <Link href={ApplicationPaths.Register}>Register</Link>
    },
    {
      key: 2,
      icon: <LoginOutlined />,
      label: <Link href={ApplicationPaths.Login}>Login</Link>
    },
  ]
  
  return (
    <AntdLayout
      style={{
        minHeight: '100vh',
      }}
    >

      <Sider 
        collapsible 
        collapsed={collapsed} 
        onCollapse={(value) => setCollapsed(value)}
        theme="light"
      >
        <SiderLogoWrapper>
          <SiderLogoLink className="navbar-brand" href="/" >
            {collapsed ? (
              <FadeIn>
                <Icon icon={faHouse} />
              </FadeIn>
            ) : (
              <FadeIn>
                CarShop
              </FadeIn>
            )}
          </SiderLogoLink>
        </SiderLogoWrapper>

        <Menu mode="inline" items={siderMenuItems} />

        {userData && !collapsed && (
          <SiderInfoWrapper>
            <FadeIn>
              <Avatar
                size={100}
                src={userData?.imgSrc}
              />
              <LabelFormItem 
                label={'Name'} 
                labelValue={userData?.firstName}
                column
              />
              <LabelFormItem 
                label={'Surname'} 
                labelValue={userData?.lastName}
                column
              />
              <LabelFormItem 
                label={'Username'} 
                labelValue={userData?.userName}
                column
              />
              <LabelFormItem 
                label={'Role'} 
                labelValue={userData?.role}
                column
              />
              <LabelFormItem 
                label={'Balance'} 
                labelValue={userData?.balance}
                column
                currency
              />
            </FadeIn>
          </SiderInfoWrapper>
        )}
      </Sider>

      <AntdLayout>
        
        {/* <NavMenu /> */}

        <header>
          <Menu 
            items={mainMenuItems} 
            mode="horizontal"
            selectedKeys={[activeLink]}
          />
        </header>

        <Content
          style={{
            margin: '0 16px',
          }}
        >
          <Container>
            {children}
          </Container>
        </Content>

        <Footer
          style={{
            textAlign: 'center',
          }}
        >
          CarShop ©2023 Created for Qualification Project by nt21006
        </Footer>

      </AntdLayout>

    </AntdLayout>
  )
}
