import React, { useState, useEffect, useContext } from 'react';
import { useNavigate} from 'react-router-dom';
import {
  Button,
  Label,
  LabelFormItem,
  List
} from '../ui';
import { 
  StyledPage
} from '../styles/layout/form';
import { UserDataContext } from '../contexts/UserDataProvider'

export const RentSubmissionList = () => {

  const [isAdmin, setIsAdmin] = useState(false)
  const { data: userData } = useContext(UserDataContext)
  const navigate = useNavigate()

  useEffect(() => {
    if (userData?.role === 'Admin') {
      setIsAdmin(true)
    }
  }, [userData]);

  const renderTitle = (item) => {
    return (
      <>
        {`${item.mark !== 'Other' ? item.mark : ''} ${item.model}`}
      </>
    )
  }

  const renderDescription = (item) => {
    return (
      <>
        <Label 
          label={item.category} 
          listItem 
          extraBold 
        />
        <LabelFormItem 
          label={'Seller'} 
          labelValue={item.user.userName}
        />
        <br />
        <LabelFormItem 
          label={'Technical passport number'} 
          labelValue={item.aplNr}
        />
        <LabelFormItem 
          label={'License plate number'} 
          labelValue={item.regNr}
        />
        <LabelFormItem 
          label={'Status'} 
          labelValue={item.status}
        />
        <LabelFormItem 
          label={'Admin status'} 
          labelValue={item.adminStatus}
        />
      </>
    )
  }

  const renderAction = (item) => {
    switch (item.adminStatus) {
      case 'Confirmed': 
        return isAdmin ? (
            <>This vehicle is confirmed</>
          ) : (
            <Button 
              type="link" 
              label="Create rental item" 
              size="small"
              onClick={() => navigate(`/new-rent-item`, { state: { rentSubmissionId: item.id } })}
            />
          )

      case 'Blocked': 
        return <>This vehicle is blocked</>
      
      default: 
        switch (item.status) {
          case 'Submitted':
            return isAdmin ? (
                <Button 
                  type="link" 
                  label="Change status" 
                  size="small"
                  onClick={() => navigate(`/rent-submission/${item.id}`)} 
                />
              ) : (
                <>Wait until administrator approves your submission</>
              )

          case 'Draft':
            return <>Submit this item to administrator to recieve approval</>

          default:
            return <>This vehicle is cancelled</>
        }
    }
  }

  const filterItems = {
    addUser: true,
    addStatus: true,
    items: [
      [
        {
          label: 'Technical passport number',
          name: 'AplNr',
          type: 'input'
        },
        {
          label: 'License plate number',
          name: 'RegNr',
          type: 'input'
        }
      ],
      [
        {
          label: 'Mark',
          name: 'Mark',
          type: 'select',
          apiUrl: 'api/marks'
        },
        {
          label: 'Model',
          name: 'Model',
          type: 'input'
        },
        {
          label: 'Category',
          name: 'Category',
          type: 'select',
          apiUrl: 'api/categories'
        }
      ]
    ]
  }

  const sortItems = [
    {
      label: 'Status',
      value: 'Status'
    },
    {
      label: 'Technical passport number',
      value: 'AplNr'
    },
    {
      label: 'License plate number',
      value: 'RegNr'
    },
    {
      label: 'Category',
      value: 'Category'
    },
    {
      label: 'Mark',
      value: 'Mark'
    },
    {
      label: 'Model',
      value: 'Model'
    },
    {
      label: 'Mileage',
      value: 'Mileage'
    },
    {
      label: 'Year',
      value: 'Year'
    }
  ]

  return (
    <StyledPage>
      <List
        title={isAdmin ? 'Rent submissions' : 'Your Rent submissions'}
        url={'rent-submission'}
        apiUrl={'api/rent-submissions'}
        filterItems={filterItems}
        sortItems={sortItems}
        renderTitle={renderTitle}
        renderDescription={renderDescription}
        renderAction={renderAction}
      />
    </StyledPage>
  )
}
