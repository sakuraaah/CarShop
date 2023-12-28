import React, { useState, useEffect, useContext } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Form as AntdForm, message } from 'antd';
import dayjs from 'dayjs';
import {
  Button,
  Form,
  ImageUpload,
  Input,
  InputNumber,
  Label,
  LabelFormItem,
  Loader,
  Modal,
  Table,
  SideBySide,
} from '../ui';
import { 
  BorderBottom,
  ButtonList,
  Color,
  FormHeader,
  StyledPage, 
  StyledWrapper,
} from '../styles/layout/form';
import useQueryApiClient from '../utils/useQueryApiClient';
import { UserDataContext } from '../contexts/UserDataProvider'

export const BuyerOrderList = () => {
  const { refetch } = useContext(UserDataContext)

  const navigate = useNavigate()

  const { refetch: finishRentItem, isLoading: finishOrderLoading } = useQueryApiClient({
    request: {
      url: `api/user-data/rent-orders/{id}/finish`,
      method: 'POST'
    },
    onSuccess: () => {
      refetch()
      refetchOrders()
    }
  })

  const { data: orders, refetch: refetchOrders, isLoading: getOrdersLoading } = useQueryApiClient({
    request: {
      url: `api/user-data/rent-orders`,
      method: 'GET'
    }
  })

  const orderColumns = [
    {
      key: 1,
      dataIndex: 'rentItem',
      title: 'Rented item'
    },
    {
      key: 2,
      dataIndex: 'status',
      title: 'Status',
      render: (status) => (
        <Color className={status === 'Done' ? 'green' : 'yellow'}>
          {status}
        </Color>
      )
    },
    {
      key: 3,
      dataIndex: 'startTime',
      title: 'Start time',
      render: (date) => dayjs(date).format('DD-MM-YYYY HH:MM')
    },
    {
      key: 4,
      dataIndex: 'endTime',
      title: 'End time',
      render: (date) => date ? dayjs(date).format('DD-MM-YYYY HH:MM') : '-'
    },
    {
      key: 5,
      dataIndex: 'rentItemId',
      title: 'Vehicle',
      render: (id) => (
        <Button 
          type="link" 
          label="View vehicle"
          size="small"
          onClick={() => navigate(`/rent-item/${id}`)} 
        />
      )
    },
    {
      key: 5,
      dataIndex: 'id',
      title: 'Actions',
      render: (id, order) => order.status === 'Pending' ? (
        <Button 
          label="Finish"
          size="small"
          onClick={() => finishRentItem({ id: id })} 
        />
      ) : '-'
    },
  ]

  return (
    <StyledPage>

      <FormHeader>
        <Label 
          label={'Rent orders'} 
          extraBold 
        />
      </FormHeader>

      <StyledWrapper>
        <Label label={'Your rent orders'} extraBold />

        <BorderBottom />

        <Table 
          dataSource={orders?.data}
          columns={orderColumns}
          loading={getOrdersLoading || finishOrderLoading}
        />
      </StyledWrapper>

      <StyledWrapper>
        <ButtonList>
          <Button 
            onClick={() => navigate(-1)} 
            label={'Return'}
          />
        </ButtonList>
      </StyledWrapper>
    </StyledPage>
  )
}
