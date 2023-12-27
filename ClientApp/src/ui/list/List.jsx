import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { List as AntdList } from 'antd';
import useQueryApiClient from '../../utils/useQueryApiClient';
import { DefaultFilters } from '../../components/filter/DefaultFilters';
import {
  Avatar,
  Button,
  Image,
  Form,
  Label,
  Loader,
  Select,
  SideBySide,
  Switch
} from '../../ui';
import {
  BorderBottom,
  FormHeader
} from '../../styles/layout/form';

export const List = ({
  title = 'List',
  url,
  apiUrl,
  button,
  sortItems,
  filterItems,
  dataSource = [],
  renderTitle,
  renderDescription,
  ...props
}) => {
  const navigate = useNavigate()

  const [currentPage, setCurrentPage] = useState(1)
  const [orderBy, setOrderBy] = useState()
  const [orderDirection, setOrderDirection] = useState('desc')
  const [filters, setFilters] = useState({})

  const StyledListItem = styled(AntdList.Item)`
    display: flex !important;
    flex-direction: column;
    gap: 20px;
    margin-block-start: 40px !important;

    .ant-image {
      order: 1;
    }

    .ant-list-item-meta {
      order: 2;
      width: 100%;
    }
  `;

  const defaultGrid = { 
    gutter: 32, 
    column: 4
  }

  const defaultPagination = { 
    position: 'both', 
    align: 'start'
  }

  useEffect(() => {
    if (apiUrl) {
      getData({
        page: currentPage,
        orderBy: orderBy,
        orderDirection: orderDirection,
        ...filters
      })
    }
  }, [apiUrl, currentPage, orderBy, orderDirection, filters])

  const { data: fetchedData, appendData: getData, isLoading } = useQueryApiClient({
    request: {
      url: apiUrl,
      method: 'GET',
      disableOnMount: true
    }
  });

  const renderItem = (item, index) => {
    return (
      <StyledListItem
        extra={
          <Image
            className="list-image"
            alt="product image"
            src={item.imgSrc}
          />
        }
      >
        <AntdList.Item.Meta
          avatar={
            <Avatar src={item.user.imgSrc} />
          }
          title={<a href={`${url}/${item.id}`}>{renderTitle(item)}</a>}
          description={renderDescription(item)}
        />
      </StyledListItem>
    )
  }

  return (
    <>
      <FormHeader>
        <Label 
          label={`${title}`} 
          extraBold 
        />
      </FormHeader>

      <BorderBottom />

      {button && (
        <Button 
          onClick={() => navigate(button.url)}
          label={button.label} 
          style={{
            marginBottom: '30px',
          }}
        />
      )}

      {sortItems && (
        <>
          <Form>
            <SideBySide
              left={
                <Select
                  name="orderBy"
                  label={'Sort by'}
                  value={orderBy}
                  options={sortItems}
                  onChange={(value) => setOrderBy(value)}
                  defaultValue={{
                    label: 'Date',
                    value: 'Created'
                  }}
                />
              }
              right={
                <Switch
                  label={'Direction'}
                  name={'orderDirection'}
                  checkedChildren='Asc' 
                  unCheckedChildren='Desc'
                  onChange={(e) => setOrderDirection(e ? 'asc' : 'desc')}
                />
              }
            />
          </Form>

          <BorderBottom />
        </>
      )}

      {filterItems && (
        <>
          <DefaultFilters
            filterItems={filterItems}
            setFilters={setFilters}
          />

          <BorderBottom />
        </>
      )}

      <Loader loading={isLoading} >
        <AntdList
          { ...props }
          dataSource={!!apiUrl ? fetchedData?.data?.results : dataSource}
          renderItem={renderItem}
          grid={props.grid ?? defaultGrid}
          pagination={{
            ...(props.pagination ?? defaultPagination),
            total: !!apiUrl ? fetchedData?.data?.total : dataSource.length,
            pageSize: !!apiUrl ? fetchedData?.data?.pageSize : props.pagination.pageSize ?? 40,
            current: currentPage,
            onChange: (page) => setCurrentPage(page),
            showTotal: (total, range) => `${range[0]}-${range[1]} of ${total} items`
          }}
        />
      </Loader>
    </>
  );
};
